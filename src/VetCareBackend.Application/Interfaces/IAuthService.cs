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
    }
}
