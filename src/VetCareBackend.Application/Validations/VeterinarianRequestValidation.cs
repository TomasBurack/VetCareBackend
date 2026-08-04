using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;
using VetCareBackend.Application.dtos.Requests;
using VetCareBackend.Domain.Enums;

namespace VetCareBackend.Application.Validations
{
    public class VeterinarianRequestValidation : AbstractValidator<VeterinarianRequest>
    {
        public VeterinarianRequestValidation()
        {
            RuleFor(vet => vet.FirstName)
                .NotEmpty().WithMessage("El nombre es obligatorio.")
                .MinimumLength(3).WithMessage("El nombre debe tener al menos 3 caracteres.")
                .MaximumLength(15).WithMessage("El nombre no puede superar los 15 caracteres.");
            RuleFor(vet => vet.LastName)
                .NotEmpty().WithMessage("El apellido es obligatorio.")
                .MinimumLength(3).WithMessage("El apellido debe tener al menos 3 caracteres.")
                .MaximumLength(15).WithMessage("El apellido no puede superar los 15 caracteres.");
            RuleFor(vet => vet.Dni)
                .NotEmpty().WithMessage("El DNI es obligatorio.")
                .Length(8).WithMessage("El DNI debe tener exactamente 8 caracteres.")
                .Must(dni => dni.All(char.IsDigit)).WithMessage("El DNI debe contener solo números.");
            RuleFor(vet => vet.Email)
                .NotEmpty().WithMessage("El email es obligatorio.")
                .EmailAddress().WithMessage("El formato del email no es válido.");
            RuleFor(vet => vet.PhoneNumber)
                .NotEmpty().WithMessage("El teléfono es obligatorio.")
                .MinimumLength(9).WithMessage("El teléfono debe tener al menos 9 caracteres.")
                .MaximumLength(11).WithMessage("El teléfono no puede superar los 11 caracteres.")
                .Must(pn => pn.All(char.IsDigit)).WithMessage("El teléfono debe contener solo números.");
            RuleFor(vet => vet.Enrollment)
                .NotEmpty().WithMessage("La matrícula es obligatoria.")
                .Length(4).WithMessage("La matrícula debe tener exactamente 4 caracteres.")
                .Must(enrollment => enrollment.All(char.IsDigit)).WithMessage("La matrícula debe contener solo números.");
            RuleFor(vet => vet.Speciality)
                .NotEmpty().WithMessage("La especialidad es obligatoria.")
                .IsInEnum<VeterinarianRequest, Speciality>()
                .WithMessage("La especialidad veterinaria no es válida. Por favor, seleccioná una opción válida de la lista.");
            RuleFor(vet => vet.Password)
                .NotEmpty().WithMessage("La contraseña es obligatoria.")
                .MinimumLength(8).WithMessage("La contraseña debe tener al menos 8 caracteres.")
                .Matches("[A-Z]").WithMessage("La contraseña debe contener al menos una letra mayúscula.")
                .Matches("[a-z]").WithMessage("La contraseña debe contener al menos una letra minúscula.")
                .Matches("[0-9]").WithMessage("La contraseña debe contener al menos un número.")
                .Matches("[^a-zA-Z0-9]").WithMessage("La contraseña debe contener al menos un carácter especial.");
        }
    }
}