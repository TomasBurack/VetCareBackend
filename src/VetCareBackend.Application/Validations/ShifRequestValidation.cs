using FluentValidation;
using System;
using VetCareBackend.Application.dtos.Requests;
using VetCareBackend.Application.Interfaces;

namespace VetCareBackend.Application.Validators
{
    public class ShiftRequestValidator : AbstractValidator<ShiftRequest>
    {
        public ShiftRequestValidator(IShiftRepository shiftRepository)
        {
            RuleFor(x => x.Enrollment)
                .NotEmpty().WithMessage("Enrollment is required.");

            RuleFor(x => x.PetId)
                .NotEmpty().WithMessage("PetId is required.");

            RuleFor(x => x.Description)
                .NotEmpty().WithMessage("Description is required.");

            RuleFor(x => x.DateShift)
                .NotEmpty().WithMessage("DateShift is required.")
                .GreaterThan(DateTime.Now).WithMessage("The shift date cannot be in the past.")
                .Must((request, dateShift) => !shiftRepository.GetAll()
                    .Any(s => s.Enrollment == request.Enrollment && s.DateShift == dateShift))
                .WithMessage("The veterinarian already has a shift on that date and time.");
        }
    }
}
// Maniana corrijo lo de l repo en el fluent