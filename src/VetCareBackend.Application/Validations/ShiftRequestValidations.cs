using FluentValidation;
using FluentValidation.Validators;
using System;
using System.Collections.Generic;
using System.Text;
using VetCareBackend.Application.dtos.Requests;

namespace VetCareBackend.Application.Validations
{
    public class ShiftRequestValidations : AbstractValidator<ShiftRequest>
    {
        public ShiftRequestValidations()
        {
            RuleFor(request => request.DateShift)
                .GreaterThan(DateTime.UtcNow)
                .WithMessage("La fecha del turno no es válida")
                .LessThanOrEqualTo(DateTime.UtcNow.AddYears(1))
                .WithMessage("La fecha del turno no puede ser mayor a 1 año a futuro.");
            RuleFor(request => request.DateShift.TimeOfDay)
                .InclusiveBetween(new TimeSpan(8, 0, 0), new TimeSpan(20, 0, 0))
                .WithMessage("El horario del turno debe estar entre las 08:00 y las 20:00.");
            RuleFor(request => request.Enrollment)
                .NotEmpty().WithMessage("La matrícula es obligatoria.")
                .Length(4).WithMessage("La matrícula debe tener exactamente 4 caracteres.")
                .Must(enrollment => enrollment.All(char.IsDigit)).WithMessage("La matrícula debe contener solo números.");
            RuleFor(request => request.PetId)
                .NotEmpty().WithMessage("La mascota es obligatoria");
            RuleFor(request => request.Description)
                .NotEmpty().WithMessage("La descripción es obligatoria");
        }
    }
}
