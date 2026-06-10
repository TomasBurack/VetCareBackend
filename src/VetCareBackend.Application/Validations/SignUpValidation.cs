using FluentValidation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using VetCareBackend.Application.dtos.Requests;

namespace VetCareBackend.Application.Validations
{
    public class SignUpValidator : AbstractValidator<SignUpRequest>
    {
        public SignUpValidator()
        {
            RuleFor(ur => ur.FirstName)
                .NotEmpty().WithMessage("First name is required.")
                .MinimumLength(3).WithMessage("First name must be at least 3 characters long.")
                .MaximumLength(15).WithMessage("First name cannot exceed 15 characters.");
            RuleFor(ur => ur.LastName)
                .NotEmpty().WithMessage("Last name is required.")
                .MinimumLength(3).WithMessage("Last name must be at least 3 characters long.")
                .MaximumLength(15).WithMessage("Last name cannot exceed 15 characters.");
            RuleFor(ur => ur.Dni)
                .NotEmpty().WithMessage("DNI is required.")
                .Length(8).WithMessage("DNI must be exactly 8 characters long.")
                .Must(dni => dni.All(char.IsDigit)).WithMessage("DNI must contain only numbers.");
            RuleFor(ur => ur.Email)
                .NotEmpty().WithMessage("Email address is required.")
                .EmailAddress().WithMessage("The email address format is not valid.");
            RuleFor(ur => ur.PhoneNumber)
                .NotEmpty().WithMessage("Phone number is required.")
                .MinimumLength(9).WithMessage("Phone number must be at least 9 characters long.")
                .MaximumLength(11).WithMessage("Phone number cannot exceed 11 characters.")
                .Must(pn => pn.All(char.IsDigit)).WithMessage("Phone number must contain only numbers.");
        }
    }
}
