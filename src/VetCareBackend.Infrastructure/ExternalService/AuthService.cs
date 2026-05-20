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
using VetCareBackend.Application.Interfaces;
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

        public AuthResponse SingUp(SingUpRequest request)
        {
            if (!Regex.IsMatch(request.Email, @"^[^@\s]+@[^@\s]+\.[^@\s]+$"))
                throw new Exception($"The email {request.Email} is invalid"); //hacer un manejo de excepciones y cambiarlo
            
            bool emailUsed = _context.Clients.Any(c => c.Email == request.Email) || _context.Veterinarians.Any(v=> v.Email == request.Email) || _context.Administrators.Any(a=> a.Email == request.Email);
            
            if (emailUsed)
                throw new Exception($"The email {request.Email} is already in use"); //tambien hacer mano de excepciones

            string hashedPassword = BCrypt.Net.BCrypt.HashPassword(request.Password);
            Guid id = Guid.NewGuid();
            string rol;

            if (request.Role == "Client")
            {
                var client = new Client
                {
                    FirstName = request.FirstName,
                    LastName = request.LastName,
                    Dni = request.Dni,
                    Email = request.Email,
                    PhoneNumber = request.PhoneNumber,
                    Password = hashedPassword,
                    Role = Role.Client
                };
                _context.Clients.Add(client);
                rol = "Client";
            } else if( request.Role == "Veterinarian")
            {
                var veterinarian = new Veterinarian
                {
                    FirstName = request.FirstName,
                    LastName = request.LastName,
                    Dni = request.Dni,
                    Email = request.Email,
                    PhoneNumber = request.PhoneNumber,
                    Password = hashedPassword,
                    Role = Role.Veterinarian
                };
                _context.Veterinarians.Add(veterinarian);
                rol = "Veterinarian";
            } else
            {
                var administrator = new Administrator
                {
                    FirstName = request.FirstName,
                    LastName = request.LastName,
                    Dni = request.Dni,
                    Email = request.Email,
                    PhoneNumber = request.PhoneNumber,
                    Password = hashedPassword,
                    Role = Role.Administrator
                };
                _context.Administrators.Add(administrator);
                rol = "Administrator";
            }

            try
            {
                _context.SaveChanges();
            }catch (DbUpdateException ex)
            {
                throw new Exception("There was an error saving the user in the database", ex); //tambien hacer manejo de error y cambiarlo
            }

            return new AuthResponse { };
        }

        public AuthResponse SingIn(SingInRequest request)
        {
            return new AuthResponse { };
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
