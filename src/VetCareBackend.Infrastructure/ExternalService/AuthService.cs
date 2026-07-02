using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Data;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Security.Claims;
using System.Text;
using System.Text.RegularExpressions;
using VetCareBackend.Application.dtos.Requests;
using VetCareBackend.Application.dtos.Responses;
using VetCareBackend.Application.Exceptions;
using VetCareBackend.Application.Interfaces;
using VetCareBackend.Application.Mapper;
using VetCareBackend.Application.Validations;
using VetCareBackend.Domain.Entities;
using VetCareBackend.Domain.Enums;

namespace VetCareBackend.Infrastructure.ExternalService
{
    public class AuthService : IAuthService
    {
        private readonly VetCareDbContext _context;
        private readonly IConfiguration _configuration;
        private readonly IMailService _mailService;
        public AuthService(VetCareDbContext context, IConfiguration configuration, IMailService mailService)
        {
            _context = context;
            _configuration = configuration;
            _mailService = mailService;
        }
        public async Task ForgotPassword(ForgotPasswordRequest request)
        {
            bool emailExists =
                await _context.Clients.AnyAsync(c => c.Email == request.Email && !c.IsDeleted) ||
                await _context.Veterinarians.AnyAsync(v => v.Email == request.Email && !v.IsDeleted) ||
                await _context.Administrators.AnyAsync(a => a.Email == request.Email && !a.IsDeleted) ||
                await _context.Sysadmins.AnyAsync(s => s.Email == request.Email && !s.IsDeleted);

            if (!emailExists) return;
            string token = Guid.NewGuid().ToString("N");

            var resetToken = new PasswordResetToken
            {
                Id = Guid.NewGuid(),
                Token = token,
                Email = request.Email,
                ExpiresAt = DateTime.UtcNow.AddMinutes(15),
                isUsed = false
            };

            _context.PasswordResetTokens.Add(resetToken);
            await _context.SaveChangesAsync();

            string resetLink = $"https://tuapp.com/reset-password?token={token}";
            string body = $"Hola,\n\nPara restablecer tu contraseña hacé click en el siguiente link:\n\n{resetLink}\n\nEste link vence en 15 minutos.\n\nSi no solicitaste esto, ignore este mensaje.";

            await _mailService.SendEmail(request.Email, request.Email, "Recuperacion de contraseña - VetCare", body);
        }

        public async Task ResetPassword(ResetPasswordRequest request)
        {
            var resetToken = await _context.PasswordResetTokens
                .FirstOrDefaultAsync(t => t.Token == request.Token);

            if (resetToken == null)
                throw new NotFoundException("El token no es valido");

            if (resetToken.isUsed)
                throw new ValidationException("El token ya fue utilizado");

            if (resetToken.ExpiresAt < DateTime.UtcNow)
                throw new ValidationException("El token ha espirado");

            User? user = 
                (User?) await _context.Clients.FirstOrDefaultAsync(c => c.Email == resetToken.Email && !c.IsDeleted) ??
                (User?) await _context.Veterinarians.FirstOrDefaultAsync(v => v.Email == resetToken.Email && !v.IsDeleted) ??
                (User? )await _context.Administrators.FirstOrDefaultAsync(a => a.Email == resetToken.Email && !a.IsDeleted) ??
                (User?) await _context.Sysadmins.FirstOrDefaultAsync(s => s.Email == resetToken.Email && !s.IsDeleted);

            if (user == null)
                throw new NotFoundException("The user associated with the token could not be found.");

            ResetPasswordRequestValidations validation = new ResetPasswordRequestValidations();

            if(!validation.Validate(request).IsValid)
            {
                throw new ValidationException(validation.Validate(request).ToString("~"));
            }

            if(BCrypt.Net.BCrypt.Verify(request.NewPassword, user.Password))
            {
                throw new ValidationException("The new password cannot be the same as the previous one.");
            }

            user.Password = BCrypt.Net.BCrypt.HashPassword(request.NewPassword); ;
            resetToken.isUsed = true;

            await _context.SaveChangesAsync();

        }
        public async Task<AuthResponse> SignUp(SignUpRequest request)
        {
            bool emailUsed = await _context.Clients.AnyAsync(c => c.Email == request.Email && !c.IsDeleted)
                || await _context.Veterinarians.AnyAsync(v => v.Email == request.Email && !v.IsDeleted)
                || await _context.Administrators.AnyAsync(a => a.Email == request.Email && !a.IsDeleted);

            if (emailUsed)
            {
                throw new ConflictException($"The email {request.Email} is already in use");
            }

            Guid id = Guid.NewGuid();
            string dtoRole = "Client";
            SignUpValidator validation = new SignUpValidator();
            if (!validation.Validate(request).IsValid)
            {
                throw new ValidationException(validation.Validate(request).ToString("~"));
            }

            string hashedPassword = BCrypt.Net.BCrypt.HashPassword(request.Password);
            request.Password = hashedPassword;
            
            var client = UserMapper.ToEntity<Client>(request, dtoRole, id);

            _context.Clients.Add(client);

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException ex)
            {
                throw new DatabaseException("There was an error saving the user in the database", ex);
            }

            return new AuthResponse
            {
                Token = GenerateToken(id, request.Email, dtoRole),
                Role = dtoRole,
                UserId = id,
                Email = request.Email,
            };
        }

        public async Task<AuthResponse> SignIn(SignInRequest request)
        {
            Guid userId;
            string role;

            var client = await _context.Clients.FirstOrDefaultAsync(c => c.Email == request.Email && !c.IsDeleted);
            var veterinarian = await _context.Veterinarians.FirstOrDefaultAsync(v => v.Email == request.Email && !v.IsDeleted);
            var administrator = await _context.Administrators.FirstOrDefaultAsync(a => a.Email == request.Email && !a.IsDeleted);
            var sysadmin = await _context.Sysadmins.FirstOrDefaultAsync(s => s.Email == request.Email && !s.IsDeleted);

            if (client != null)
            {
                if (!BCrypt.Net.BCrypt.Verify(request.Password, client.Password))
                    throw new UnauthorizedException("incorrect credentials");

                userId = client.Id;
                role = "Client";
            }
            else if (veterinarian != null && !veterinarian.IsDeleted)
            {
                if (!BCrypt.Net.BCrypt.Verify(request.Password, veterinarian.Password))
                    throw new UnauthorizedException("incorrect credentials");

                userId = veterinarian.Id;
                role = "Veterinarian";
            }
            else if (administrator != null && !administrator.IsDeleted)
            {
                if (!BCrypt.Net.BCrypt.Verify(request.Password, administrator.Password))
                    throw new UnauthorizedException("incorrect credentials");

                userId = administrator.Id;
                role = "Administrator";
            }
            else if (sysadmin != null && !sysadmin.IsDeleted)
            {
                if (!BCrypt.Net.BCrypt.Verify(request.Password, sysadmin.Password))
                    throw new UnauthorizedException("incorrect credentials");
                userId = sysadmin.Id;
                role = "SysAdmin";
            }
            else
            {
                throw new UnauthorizedException("incorrect credentials");
            }

            return new AuthResponse
            {
                Token = GenerateToken(userId, request.Email, role),
                Role = role,
                UserId = userId,
                Email = request.Email
            };
        }

        private string GenerateToken(Guid UserId, string Email, string Role)
        {
            string key = _configuration["Jwt:Key"]!;
            string issuer = _configuration["Jwt:Issuer"]!;
            string audience = _configuration["Jwt:Audience"]!;
            int expirationMinutes = int.Parse(_configuration["Jwt:ExpirationMinutes"]!);

            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, UserId.ToString()),
                new Claim(JwtRegisteredClaimNames.Email, Email),
                new Claim(ClaimTypes.Role, Role),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Iat,
                    DateTimeOffset.UtcNow.ToUnixTimeSeconds().ToString(),
                    ClaimValueTypes.Integer64)
            };

            var token = new JwtSecurityToken(
                issuer: issuer,
                audience: audience,
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(expirationMinutes),
                signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
