using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;
using VetCareBackend.Application.dtos.Requests;

namespace VetCareBackend.Application.Validations
{
    public class ResetPasswordRequestValidations : AbstractValidator<ResetPasswordRequest>
    {
        public ResetPasswordRequestValidations() 
        {
            RuleFor(r => r.NewPassword).NotEmpty().WithMessage("La nueva contraseña es obligatoria.")
                .MinimumLength(8).WithMessage("La nueva contraseña debe tener al menos 8 caracteres.")
                .Matches("[A-Z]").WithMessage("La nueva contraseña debe contener al menos una letra mayúscula.")
                .Matches("[a-z]").WithMessage("La nueva contraseña debe contener al menos una letra minúscula.")
                .Matches("[0-9]").WithMessage("La nueva contraseña debe contener al menos un número.")
                .Matches("[^a-zA-Z0-9]").WithMessage("La nueva contraseña debe contener al menos un carácter especial.");
        }
    }
}
