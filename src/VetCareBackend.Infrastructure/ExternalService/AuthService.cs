using Microsoft.AspNetCore.DataProtection;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using OtpNet;
using System;
using System.Collections.Generic;
using System.Data;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Security.Cryptography;
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
        private const string TwoFactorPurpose = "2fa-pending";
        private const int TwoFactorPendingTokenMinutes = 5;
        private const int MaxTwoFactorAttempts = 3;
        private static readonly TimeSpan TwoFactorAttemptsWindow = TimeSpan.FromMinutes(TwoFactorPendingTokenMinutes);

        private readonly VetCareDbContext _context;
        private readonly IConfiguration _configuration;
        private readonly IMailService _mailService;
        private readonly IDataProtector _dataProtector;
        private readonly IMemoryCache _memoryCache;
        public AuthService(VetCareDbContext context, IConfiguration configuration, IMailService mailService, IDataProtectionProvider dataProtectionProvider, IMemoryCache memoryCache)
        {
            _context = context;
            _configuration = configuration;
            _mailService = mailService;
            _dataProtector = dataProtectionProvider.CreateProtector("VetCareBackend.TwoFactorSecret");
            _memoryCache = memoryCache;
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

            string frontendBaseUrl = _configuration["FrontendBaseUrl"];
            string resetLink = $"{frontendBaseUrl}/reset-password?token={token}";
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
            var (user, role) = await FindUserByEmail(request.Email)
                ?? throw new UnauthorizedException("incorrect credentials");

            if (!BCrypt.Net.BCrypt.Verify(request.Password, user.Password))
                throw new UnauthorizedException("incorrect credentials");

            if (user.TwoFactorEnabled)
            {
                return new AuthResponse
                {
                    TwoFactorRequired = true,
                    PendingTwoFactorToken = GeneratePendingTwoFactorToken(user.Id),
                    Role = role,
                    UserId = user.Id,
                    Email = user.Email
                };
            }

            return new AuthResponse
            {
                Token = GenerateToken(user.Id, user.Email, role),
                Role = role,
                UserId = user.Id,
                Email = user.Email
            };
        }

        public async Task<TwoFactorSetupResponse> BeginTwoFactorEnrollment(Guid userId)
        {
            var (user, _) = await FindUserById(userId)
                ?? throw new NotFoundException("User not found");

            var secretKey = KeyGeneration.GenerateRandomKey(20);
            var base32Secret = Base32Encoding.ToString(secretKey);

            user.TwoFactorSecret = _dataProtector.Protect(base32Secret);
            await _context.SaveChangesAsync();

            string issuer = _configuration["Jwt:Issuer"]!;
            string otpAuthUri = $"otpauth://totp/{Uri.EscapeDataString(issuer)}:{Uri.EscapeDataString(user.Email)}" +
                $"?secret={base32Secret}&issuer={Uri.EscapeDataString(issuer)}&digits=6&period=30";

            return new TwoFactorSetupResponse { OtpAuthUri = otpAuthUri };
        }

        public async Task ConfirmTwoFactorEnrollment(Guid userId, string code)
        {
            var (user, _) = await FindUserById(userId)
                ?? throw new NotFoundException("User not found");

            if (string.IsNullOrEmpty(user.TwoFactorSecret))
                throw new ValidationException("Two-factor enrollment was not started");

            string attemptsCacheKey = $"2fa-confirm-attempts:{userId}";
            EnforceAttemptLimit(attemptsCacheKey);

            if (!ValidateTotpCode(user.TwoFactorSecret, code))
            {
                RegisterFailedAttempt(attemptsCacheKey);
                throw new UnauthorizedException("Invalid verification code");
            }

            _memoryCache.Remove(attemptsCacheKey);

            user.TwoFactorEnabled = true;
            await _context.SaveChangesAsync();
        }

        public async Task<AuthResponse> VerifyTwoFactor(string pendingToken, string code)
        {
            var userId = ValidatePendingTwoFactorToken(pendingToken)
                ?? throw new UnauthorizedException("Invalid or expired session");

            var (user, role) = await FindUserById(userId)
                ?? throw new UnauthorizedException("incorrect credentials");

            if (!user.TwoFactorEnabled || string.IsNullOrEmpty(user.TwoFactorSecret))
                throw new ValidationException("Two-factor authentication is not enabled for this user");

            string attemptsCacheKey = $"2fa-verify-attempts:{userId}";
            EnforceAttemptLimit(attemptsCacheKey);

            bool isValid = ValidateTotpCode(user.TwoFactorSecret, code);

            if (!isValid)
            {
                RegisterFailedAttempt(attemptsCacheKey);
                throw new UnauthorizedException("Invalid verification code");
            }

            _memoryCache.Remove(attemptsCacheKey);

            return new AuthResponse
            {
                Token = GenerateToken(user.Id, user.Email, role),
                Role = role,
                UserId = user.Id,
                Email = user.Email
            };
        }

        public async Task DisableTwoFactor(Guid userId, string password)
        {
            var (user, _) = await FindUserById(userId)
                ?? throw new NotFoundException("User not found");

            if (!BCrypt.Net.BCrypt.Verify(password, user.Password))
                throw new UnauthorizedException("incorrect credentials");

            user.TwoFactorEnabled = false;
            user.TwoFactorSecret = null;
            await _context.SaveChangesAsync();
        }

        public async Task<bool> IsTwoFactorEnabled(Guid userId)
        {
            var (user, _) = await FindUserById(userId)
                ?? throw new NotFoundException("User not found");

            return user.TwoFactorEnabled;
        }

        private void EnforceAttemptLimit(string cacheKey)
        {
            int attempts = _memoryCache.Get<int?>(cacheKey) ?? 0;
            if (attempts >= MaxTwoFactorAttempts)
                throw new UnauthorizedException("Too many failed attempts. Please try again.");
        }

        private void RegisterFailedAttempt(string cacheKey)
        {
            int attempts = (_memoryCache.Get<int?>(cacheKey) ?? 0) + 1;
            _memoryCache.Set(cacheKey, attempts, TwoFactorAttemptsWindow);
        }

        private async Task<(User User, string Role)?> FindUserByEmail(string email)
        {
            var client = await _context.Clients.FirstOrDefaultAsync(c => c.Email == email && !c.IsDeleted);
            if (client != null) return (client, "Client");

            var veterinarian = await _context.Veterinarians.FirstOrDefaultAsync(v => v.Email == email && !v.IsDeleted);
            if (veterinarian != null) return (veterinarian, "Veterinarian");

            var administrator = await _context.Administrators.FirstOrDefaultAsync(a => a.Email == email && !a.IsDeleted);
            if (administrator != null) return (administrator, "Administrator");

            var sysadmin = await _context.Sysadmins.FirstOrDefaultAsync(s => s.Email == email && !s.IsDeleted);
            if (sysadmin != null) return (sysadmin, "SysAdmin");

            return null;
        }

        private async Task<(User User, string Role)?> FindUserById(Guid id)
        {
            var client = await _context.Clients.FirstOrDefaultAsync(c => c.Id == id && !c.IsDeleted);
            if (client != null) return (client, "Client");

            var veterinarian = await _context.Veterinarians.FirstOrDefaultAsync(v => v.Id == id && !v.IsDeleted);
            if (veterinarian != null) return (veterinarian, "Veterinarian");

            var administrator = await _context.Administrators.FirstOrDefaultAsync(a => a.Id == id && !a.IsDeleted);
            if (administrator != null) return (administrator, "Administrator");

            var sysadmin = await _context.Sysadmins.FirstOrDefaultAsync(s => s.Id == id && !s.IsDeleted);
            if (sysadmin != null) return (sysadmin, "SysAdmin");

            return null;
        }

        private bool ValidateTotpCode(string protectedSecret, string code)
        {
            if (string.IsNullOrWhiteSpace(code))
                return false;

            string base32Secret = _dataProtector.Unprotect(protectedSecret);
            var totp = new Totp(Base32Encoding.ToBytes(base32Secret));
            return totp.VerifyTotp(code, out _, new VerificationWindow(previous: 1, future: 1));
        }

        private string GeneratePendingTwoFactorToken(Guid userId)
        {
            string key = _configuration["Jwt:Key"]!;
            string issuer = _configuration["Jwt:Issuer"]!;
            string audience = _configuration["Jwt:Audience"]!;

            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, userId.ToString()),
                new Claim("purpose", TwoFactorPurpose),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            var token = new JwtSecurityToken(
                issuer: issuer,
                audience: audience,
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(TwoFactorPendingTokenMinutes),
                signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        private Guid? ValidatePendingTwoFactorToken(string token)
        {
            string key = _configuration["Jwt:Key"]!;
            string issuer = _configuration["Jwt:Issuer"]!;
            string audience = _configuration["Jwt:Audience"]!;

            var handler = new JwtSecurityTokenHandler();
            var validationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidIssuer = issuer,
                ValidateAudience = true,
                ValidAudience = audience,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key))
            };

            try
            {
                var principal = handler.ValidateToken(token, validationParameters, out _);
                var purpose = principal.FindFirst("purpose")?.Value;
                if (purpose != TwoFactorPurpose)
                    return null;

                var sub = principal.FindFirst(ClaimTypes.NameIdentifier)?.Value
                    ?? principal.FindFirst(JwtRegisteredClaimNames.Sub)?.Value;
                return sub != null ? Guid.Parse(sub) : null;
            }
            catch
            {
                return null;
            }
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
