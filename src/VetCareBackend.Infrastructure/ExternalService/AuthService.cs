using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
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
        public AuthService(VetCareDbContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        public async Task<AuthResponse> SignUp(SignUpRequest request)
        {
            bool emailUsed = await _context.Clients.AnyAsync(c => c.Email == request.Email && !c.IsDeleted)
                || await _context.Veterinarians.AnyAsync(v => v.Email == request.Email && !v.IsDeleted)
                || await _context.Administrators.AnyAsync(a => a.Email == request.Email && !a.IsDeleted);
            bool dniUsed = await _context.Clients.AnyAsync(c => c.Dni == request.Dni && !c.IsDeleted)
                || await _context.Veterinarians.AnyAsync(v => v.Dni == request.Dni && !v.IsDeleted)
                || await _context.Administrators.AnyAsync(a => a.Dni == request.Dni && !a.IsDeleted);
            bool pnUsed = await _context.Clients.AnyAsync(c => c.PhoneNumber == request.PhoneNumber && !c.IsDeleted)
                || await _context.Veterinarians.AnyAsync(v => v.PhoneNumber == request.PhoneNumber && !v.IsDeleted)
                || await _context.Administrators.AnyAsync(a => a.PhoneNumber == request.PhoneNumber && !a.IsDeleted);

            if (emailUsed)
            {
                throw new ConflictException($"The email {request.Email} is already in use");
            }
            else if (dniUsed)
            {
                throw new ConflictException($"The DNI {request.Dni} is already in use");
            }
            else if (pnUsed)
            {
                throw new ConflictException($"The Phone Number {request.PhoneNumber} is already in use");
            }

            string hashedPassword = BCrypt.Net.BCrypt.HashPassword(request.Password);
            request.Password = hashedPassword;
            Guid id = Guid.NewGuid();
            string dtoRole = "Client";
            SignUpValidator validation = new SignUpValidator();
            if (!validation.Validate(request).IsValid)
            {
                throw new ValidationException(validation.Validate(request).ToString("~"));
            }
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

            var client = await _context.Clients.FirstOrDefaultAsync(c => c.Email == request.Email);
            var veterinarian = await _context.Veterinarians.FirstOrDefaultAsync(v => v.Email == request.Email);
            var administrator = await _context.Administrators.FirstOrDefaultAsync(a => a.Email == request.Email);
            var sysadmin = await _context.Sysadmins.FirstOrDefaultAsync(s => s.Email == request.Email);

            if (client != null && !client.IsDeleted)
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
