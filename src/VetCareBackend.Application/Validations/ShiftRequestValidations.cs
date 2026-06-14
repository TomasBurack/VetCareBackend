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
                .WithMessage(" the shift date is invalid ");
            RuleFor(request => request.Enrrolment)
                .NotEmpty().WithMessage("The enrollment is required.")
                .Length(4).WithMessage("Enrrolment must be exactly 4 characters long.");
            RuleFor(request => request.PetId)
                .NotEmpty().WithMessage("the pet is required");
            RuleFor(request => request.Description)
                .NotEmpty().WithMessage("descriptions is requerid");



        }
    }
}
