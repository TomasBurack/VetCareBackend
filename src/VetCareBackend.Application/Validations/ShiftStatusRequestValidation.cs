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
                .NotEmpty().WithMessage("El estado es obligatorio.")
                .IsInEnum<ShiftStatusRequest, Status>().WithMessage("El estado debe ser un valor válido.");
        }
    }
}
