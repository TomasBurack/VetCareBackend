using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Text.RegularExpressions;
using VetCareBackend.Application.dtos.Requests;
using VetCareBackend.Application.dtos.Responses;
using VetCareBackend.Application.Exceptions;
using VetCareBackend.Application.Interfaces;
using VetCareBackend.Application.Mapper;
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

        public AuthResponse SignUp(UserRequest request)
        {
            if (!Regex.IsMatch(request.Email, @"^[^@\s]+@[^@\s]+\.[^@\s]+$"))
                throw new ValidationException($"The email {request.Email} is invalid");
            
            bool emailUsed = _context.Clients.Any(c => c.Email == request.Email) || _context.Veterinarians.Any(v=> v.Email == request.Email) || _context.Administrators.Any(a=> a.Email == request.Email);
            
            if (emailUsed)
                throw new ConflictException($"The email {request.Email} is already in use"); 

            string hashedPassword = BCrypt.Net.BCrypt.HashPassword(request.Password);
            Guid id = Guid.NewGuid();

            var client = UserMapper.ToEntity<Client>(request);
               
            _context.Clients.Add(client);
            
            
            try
            {
                _context.SaveChanges();
            }catch (DbUpdateException ex)
            {
                throw new DatabaseException("There was an error saving the user in the database", ex); //tambien hacer manejo de error y cambiarlo
            }

            return new AuthResponse 
            {
                Token = GenerateToken(id,request.Email, request.Role),
                Role = request.Role,
                UserId = id,
                Email = request.Email,
            };
        }

        public AuthResponse SignIn(SignInRequest request)
        {
            Guid userId;
            string role;

            var client = _context.Clients.FirstOrDefault(c => c.Email == request.Email);
            var veterinarian = _context.Veterinarians.FirstOrDefault(v => v.Email == request.Email);
            var administrator = _context.Administrators.FirstOrDefault(a => a.Email == request.Email);
            if (client != null)
            {
                if (!BCrypt.Net.BCrypt.Verify(request.Password, client.Password))
                    throw new UnauthorizedException("incorrect credentials"); //agregar manejador de errores

                userId = client.Id;
                role = "Client";
            }
            else if(veterinarian != null)
            {
                
                if (!BCrypt.Net.BCrypt.Verify(request.Password, veterinarian.Password))
                    throw new UnauthorizedException("incorrect credentials");//agregar manejador de errores

                userId = veterinarian.Id;
                role = "Veterinarian";
            }
            else
            {
                if (administrator == null)
                    throw new UnauthorizedException("incorrect credentials");//agregar manejador de errores

                if (!BCrypt.Net.BCrypt.Verify(request.Password, administrator.Password))
                    throw new UnauthorizedException("incorrect credentials");//agregar manejador de errores

                userId = administrator.Id;
                role = "Administrator";
            }
            return new AuthResponse 
            {
                Token = GenerateToken(userId, request.Email,role),
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
