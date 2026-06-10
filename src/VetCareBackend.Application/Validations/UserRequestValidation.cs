using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;
using VetCareBackend.Application.dtos.Requests;

namespace VetCareBackend.Application.Validations
{
    public class UserRequestValidation : AbstractValidator<UserRequest>
    {
        public UserRequestValidation()
        {
            RuleFor(request => request.FirstName)
                .MinimumLength(3).WithMessage("First name must be at least 3 characters long.")
                .MaximumLength(15).WithMessage("First name cannot exceed 15 characters.")
                .When(request => !string.IsNullOrWhiteSpace(request.FirstName));
            RuleFor(request => request.LastName)
                .MinimumLength(3).WithMessage("Last name must be at least 3 characters long.")
                .MaximumLength(15).WithMessage("Last name cannot exceed 15 characters.")
                .When(request => !string.IsNullOrWhiteSpace(request.LastName));
            RuleFor(request => request.Dni)
                .Length(8).WithMessage("DNI must be exactly 8 characters long.")
                .Must(dni => dni.All(char.IsDigit)).WithMessage("DNI must contain only numbers.")
                .When(request => !string.IsNullOrWhiteSpace(request.Dni));
                
            RuleFor(request => request.Email)
                .EmailAddress().WithMessage("The email address format is not valid.")
                .When(request => !string.IsNullOrWhiteSpace(request.Email));
            RuleFor(request => request.PhoneNumber)
                .MinimumLength(9).WithMessage("Phone number must be at least 9 characters long.")
                .MaximumLength(11).WithMessage("Phone number cannot exceed 11 characters.")
                .Must(pn => pn.All(char.IsDigit)).WithMessage("Phone number must contain only numbers.")
                .When(request => !string.IsNullOrWhiteSpace(request.PhoneNumber));
        }
    }
}
