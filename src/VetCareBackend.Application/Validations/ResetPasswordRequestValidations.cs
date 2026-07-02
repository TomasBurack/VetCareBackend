using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;
using VetCareBackend.Application.dtos.Requests;

namespace VetCareBackend.Application.Validations
{
    public class ResetPasswordRequestValidations : AbstractValidator<ResetPasswordRequest>
    {
        public ResetPasswordRequestValidations() 
        {
            RuleFor(r => r.NewPassword).NotEmpty().WithMessage("New Password is required.")
                .MinimumLength(8).WithMessage("New Password must be at least 8 characters long.")
                .Matches("[A-Z]").WithMessage("New Password must contain at least one uppercase letter.")
                .Matches("[a-z]").WithMessage("New Password must contain at least one lowercase letter.")
                .Matches("[0-9]").WithMessage("New Password must contain at least one number.")
                .Matches("[^a-zA-Z0-9]").WithMessage("New Password must contain at least one special character."); 
        }
    }
}
