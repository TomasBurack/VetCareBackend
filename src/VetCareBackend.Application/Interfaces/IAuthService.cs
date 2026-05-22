using System;
using System.Collections.Generic;
using System.Text;
using VetCareBackend.Application.dtos.Requests;
using VetCareBackend.Application.dtos.Responses;

namespace VetCareBackend.Application.Interfaces
{
    public interface IAuthService
    {
        AuthResponse SignUp(SignUpRequest request); //registrarse
        AuthResponse SignIn(SignInRequest request); //iniciar sesion
    }
}
