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
        public AuthService (VetCareDbContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        public AuthResponse SignUp(SignUpRequest request)
        {
            bool emailUsed = _context.Clients.Any(c => c.Email == request.Email && !c.IsDeleted) || _context.Veterinarians.Any(v => v.Email == request.Email && !v.IsDeleted) || _context.Administrators.Any(a => a.Email == request.Email && !a.IsDeleted);
            bool dniUsed = _context.Clients.Any(c => c.Dni == request.Dni && !c.IsDeleted) || _context.Veterinarians.Any(v => v.Dni == request.Dni && !v.IsDeleted) || _context.Administrators.Any(a => a.Dni == request.Dni && !a.IsDeleted);
            bool pnUsed = _context.Clients.Any(c => c.PhoneNumber == request.PhoneNumber && !c.IsDeleted) || _context.Veterinarians.Any(v => v.PhoneNumber == request.PhoneNumber && !v.IsDeleted) || _context.Administrators.Any(a => a.PhoneNumber == request.PhoneNumber && !a.IsDeleted);
            if (emailUsed) {
                throw new ConflictException($"The email {request.Email} is already in use");
            } else if (dniUsed) {
                throw new ConflictException($"The DNI {request.Dni} is already in use");
            } else if (pnUsed){
                throw new ConflictException($"The Phone Number {request.PhoneNumber} is already in use");
            }
            string hashedPassword = BCrypt.Net.BCrypt.HashPassword(request.Password);
            request.Password = hashedPassword;
            Guid id = Guid.NewGuid();
            List<string> roles = new List<string>();
            string dtoRole = "Client";
            roles.Add(dtoRole);
            SignUpValidator validation = new SignUpValidator();
            if (!validation.Validate(request).IsValid)
            {
                throw new ValidationException(validation.Validate(request).ToString("~"));
            }
            var client = UserMapper.ToEntity<Client>(request, dtoRole, id);
            
            _context.Clients.Add(client);
            
            
            try
            {
                _context.SaveChanges();
            }catch (DbUpdateException ex)
            {
                throw new DatabaseException("There was an error saving the user in the database", ex);
            }
            return new AuthResponse 
            {
                Token = GenerateToken(id, request.Email, roles),
                Roles = roles,
                UserId = id,
                Email = request.Email,
            };
        }

        public AuthResponse SignIn(SignInRequest request)
        {

            var client = _context.Clients.FirstOrDefault(c => c.Email == request.Email && !c.IsDeleted);
            var veterinarian = _context.Veterinarians.FirstOrDefault(v => v.Email == request.Email && !v.IsDeleted);
            var administrator = _context.Administrators.FirstOrDefault(a => a.Email == request.Email && !a.IsDeleted);
            var sysadmin = _context.Sysadmins.FirstOrDefault(s => s.Email == request.Email && !s.IsDeleted);

            if (client == null && veterinarian == null && administrator == null && sysadmin == null)
            {
                throw new UnauthorizedException("incorrect credentials");
            }

            
            string hashedPassword = client?.Password ?? veterinarian?.Password ?? administrator?.Password ?? sysadmin?.Password!;

            if (!BCrypt.Net.BCrypt.Verify(request.Password, hashedPassword))
            {
                throw new UnauthorizedException("incorrect credentials");
            }
            List<string> roles = new List<string>();
            Guid userId = Guid.Empty;
            if (client != null)
            {
                userId = client.Id;
                roles.Add("Client");
            }
            if(veterinarian != null)
            {
                userId = veterinarian.Id;
                roles.Add("Veterinarian");
            }
            if (administrator != null)
            {
                userId = administrator.Id;
                roles.Add("Administrator");
            }
            if (sysadmin != null)
            {
                userId = sysadmin.Id;
                roles.Add("SysAdmin");
            }
            return new AuthResponse 
            {
                Token = GenerateToken(userId, request.Email,roles),
                Roles = roles,
                UserId = userId,
                Email = request.Email
            };
        }

        private string GenerateToken(Guid UserId, string Email, List<string> roles)
        {
            string key = _configuration["Jwt:Key"]!;
            string issuer = _configuration["Jwt:Issuer"]!;
            string audience = _configuration["Jwt:Audience"]!;
            int expirationMinutes = int.Parse(_configuration["Jwt:ExpirationMinutes"]!);

            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, UserId.ToString()),
                new Claim(JwtRegisteredClaimNames.Email, Email),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Iat,
                    DateTimeOffset.UtcNow.ToUnixTimeSeconds().ToString(),
                    ClaimValueTypes.Integer64)
            };

            foreach (var role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }

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
