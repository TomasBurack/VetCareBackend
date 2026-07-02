using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;
using VetCareBackend.Application.dtos.Requests;
using VetCareBackend.Domain.Enums;

namespace VetCareBackend.Application.Validations
{
    public class ShiftStatusRequestValidation : AbstractValidator<ShiftStatusRequest>
    {
        public ShiftStatusRequestValidation() 
        {
            RuleFor(r => r.Status)
                .NotEmpty().WithMessage("Status is required.")
                .IsInEnum<ShiftStatusRequest, Status>().WithMessage("Status must be a valid enum value.");
        }
    }
}
