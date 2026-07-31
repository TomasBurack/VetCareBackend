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
            RuleFor(request => request.DateShift.TimeOfDay)
                .InclusiveBetween(new TimeSpan(8, 0, 0), new TimeSpan(20, 0, 0))
                .WithMessage("The shift time must be between 08:00 and 20:00.");
            RuleFor(request => request.Enrollment)
                .NotEmpty().WithMessage("The enrollment is required.")
                .Length(4).WithMessage("Enrrolment must be exactly 4 characters long.")
                .Must(enrollment => enrollment.All(char.IsDigit)).WithMessage("Enrollment must contain only numbers."); 
            RuleFor(request => request.PetId)
                .NotEmpty().WithMessage("the pet is required");
            RuleFor(request => request.Description)
                .NotEmpty().WithMessage("descriptions is requerid");
        }
    }
}
