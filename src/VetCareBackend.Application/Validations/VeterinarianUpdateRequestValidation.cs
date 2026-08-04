using System;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;
using VetCareBackend.Application.dtos.Requests;
using VetCareBackend.Domain.Enums;

namespace VetCareBackend.Application.Validations
{
    public class VeterinarianUpdateRequestValidation : AbstractValidator<VeterinarianUpdateRequest>
    {
        public VeterinarianUpdateRequestValidation()
        {
            RuleFor(vet => vet.FirstName)
                .MinimumLength(3).WithMessage("El nombre debe tener al menos 3 caracteres.")
                .MaximumLength(15).WithMessage("El nombre no puede superar los 15 caracteres.")
                .When(request => !string.IsNullOrWhiteSpace(request.FirstName));
            RuleFor(vet => vet.LastName)
                .MinimumLength(3).WithMessage("El apellido debe tener al menos 3 caracteres.")
                .MaximumLength(15).WithMessage("El apellido no puede superar los 15 caracteres.")
                .When(request => !string.IsNullOrWhiteSpace(request.LastName));
            RuleFor(vet => vet.Dni)
                .Length(8).WithMessage("El DNI debe tener exactamente 8 caracteres.")
                .Must(dni => dni.All(char.IsDigit)).WithMessage("El DNI debe contener solo números.")
                .When(request => !string.IsNullOrWhiteSpace(request.Dni));
            RuleFor(vet => vet.Email)
                .EmailAddress().WithMessage("El formato del email no es válido.")
                .When(request => !string.IsNullOrWhiteSpace(request.Email));
            RuleFor(vet => vet.PhoneNumber)
                .MinimumLength(9).WithMessage("El teléfono debe tener al menos 9 caracteres.")
                .MaximumLength(11).WithMessage("El teléfono no puede superar los 11 caracteres.")
                .Must(pn => pn.All(char.IsDigit)).WithMessage("El teléfono debe contener solo números.")
                .When(request => !string.IsNullOrWhiteSpace(request.PhoneNumber));
            RuleFor(vet => vet.Enrollment)
                .Length(4).WithMessage("La matrícula debe tener exactamente 4 caracteres.")
                .Must(enrollment => enrollment.All(char.IsDigit)).WithMessage("La matrícula debe contener solo números.")
                .When(vet => !string.IsNullOrWhiteSpace(vet.Enrollment));
            RuleFor(vet => vet.Speciality)
                .IsInEnum<VeterinarianUpdateRequest, Speciality>()
                .WithMessage("La especialidad veterinaria no es válida. Por favor, seleccioná una opción válida de la lista.");
        }
    }
}
