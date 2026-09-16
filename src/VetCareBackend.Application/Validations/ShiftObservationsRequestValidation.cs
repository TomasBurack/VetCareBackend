using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;
using VetCareBackend.Application.dtos.Requests;

namespace VetCareBackend.Application.Validations
{
    public class ShiftObservationsRequestValidation : AbstractValidator<ShiftObservationsRequest>
    {
        public ShiftObservationsRequestValidation()
        {
            RuleFor(r => r.Observations)
                .NotNull().WithMessage("Las observaciones son obligatorias.")
                .MaximumLength(1000).WithMessage("Las observaciones no pueden superar los 1000 caracteres.");
        }
    }
}
