using FluentValidation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using VetCareBackend.Application.dtos.Requests;

namespace VetCareBackend.Application.Validations
{
    public class SignUpValidator : AbstractValidator<SignUpRequest>
    {
        public SignUpValidator()
        {
            RuleFor(ur => ur.FirstName)
                .NotEmpty().WithMessage("El nombre es obligatorio.")
                .MinimumLength(3).WithMessage("El nombre debe tener al menos 3 caracteres.")
                .MaximumLength(15).WithMessage("El nombre no puede superar los 15 caracteres.");
            RuleFor(ur => ur.LastName)
                .NotEmpty().WithMessage("El apellido es obligatorio.")
                .MinimumLength(3).WithMessage("El apellido debe tener al menos 3 caracteres.")
                .MaximumLength(15).WithMessage("El apellido no puede superar los 15 caracteres.");
            RuleFor(ur => ur.Dni)
                .NotEmpty().WithMessage("El DNI es obligatorio.")
                .Length(8).WithMessage("El DNI debe tener exactamente 8 caracteres.")
                .Must(dni => dni.All(char.IsDigit)).WithMessage("El DNI debe contener solo números.");
            RuleFor(ur => ur.Email)
                .NotEmpty().WithMessage("El email es obligatorio.")
                .EmailAddress().WithMessage("El formato del email no es válido.");
            RuleFor(ur => ur.PhoneNumber)
                .NotEmpty().WithMessage("El teléfono es obligatorio.")
                .MinimumLength(9).WithMessage("El teléfono debe tener al menos 9 caracteres.")
                .MaximumLength(11).WithMessage("El teléfono no puede superar los 11 caracteres.")
                .Must(pn => pn.All(char.IsDigit)).WithMessage("El teléfono debe contener solo números.");
            RuleFor(ur => ur.Password)
                .NotEmpty().WithMessage("La contraseña es obligatoria.")
                .MinimumLength(8).WithMessage("La contraseña debe tener al menos 8 caracteres.")
                .Matches("[A-Z]").WithMessage("La contraseña debe contener al menos una letra mayúscula.")
                .Matches("[a-z]").WithMessage("La contraseña debe contener al menos una letra minúscula.")
                .Matches("[0-9]").WithMessage("La contraseña debe contener al menos un número.")
                .Matches("[^a-zA-Z0-9]").WithMessage("La contraseña debe contener al menos un carácter especial.");
        }
    }
}
