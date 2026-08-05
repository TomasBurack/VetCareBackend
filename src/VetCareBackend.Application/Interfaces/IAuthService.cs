using System;
using System.Collections.Generic;
using System.Text;
using VetCareBackend.Application.dtos.Requests;
using VetCareBackend.Application.dtos.Responses;

namespace VetCareBackend.Application.Interfaces
{
    public interface IAuthService
    {
        Task<AuthResponse> SignUp(SignUpRequest request);
        Task<AuthResponse> SignIn(SignInRequest request);
        Task ForgotPassword(ForgotPasswordRequest request, string? language);
        Task ResetPassword(ResetPasswordRequest request);
        Task<TwoFactorSetupResponse> BeginTwoFactorEnrollment(Guid userId);
        Task ConfirmTwoFactorEnrollment(Guid userId, string code);
        Task<AuthResponse> VerifyTwoFactor(string pendingToken, string code);
        Task DisableTwoFactor(Guid userId, string password);
        Task<bool> IsTwoFactorEnabled(Guid userId);
    }
}
