using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;
using VetCareBackend.Application.dtos.Requests;

namespace VetCareBackend.Application.Validations
{
    public class UserRequestValidation : AbstractValidator<UserRequest>
    {
        public UserRequestValidation()
        {
            RuleFor(request => request.FirstName)
                .MinimumLength(3).WithMessage("El nombre debe tener al menos 3 caracteres.")
                .MaximumLength(15).WithMessage("El nombre no puede superar los 15 caracteres.")
                .Matches(@"^[A-Za-zÀ-ÿ\s]+$").WithMessage("El nombre solo puede contener letras.")
                .When(request => !string.IsNullOrWhiteSpace(request.FirstName));
            RuleFor(request => request.LastName)
                .MinimumLength(3).WithMessage("El apellido debe tener al menos 3 caracteres.")
                .MaximumLength(15).WithMessage("El apellido no puede superar los 15 caracteres.")
                .Matches(@"^[A-Za-zÀ-ÿ\s]+$").WithMessage("El apellido solo puede contener letras.")
                .When(request => !string.IsNullOrWhiteSpace(request.LastName));
            RuleFor(request => request.Dni)
                .Length(8).WithMessage("El DNI debe tener exactamente 8 caracteres.")
                .Must(dni => dni.All(char.IsDigit)).WithMessage("El DNI debe contener solo números.")
                .When(request => !string.IsNullOrWhiteSpace(request.Dni));

            RuleFor(request => request.Email)
                .EmailAddress().WithMessage("El formato del email no es válido.")
                .When(request => !string.IsNullOrWhiteSpace(request.Email));
            RuleFor(request => request.PhoneNumber)
                .MinimumLength(9).WithMessage("El teléfono debe tener al menos 9 caracteres.")
                .MaximumLength(11).WithMessage("El teléfono no puede superar los 11 caracteres.")
                .Must(pn => pn.All(char.IsDigit)).WithMessage("El teléfono debe contener solo números.")
                .When(request => !string.IsNullOrWhiteSpace(request.PhoneNumber));
        }
    }
}
