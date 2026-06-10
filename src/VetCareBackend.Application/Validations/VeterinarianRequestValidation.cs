using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;
using VetCareBackend.Application.dtos.Requests;
using VetCareBackend.Domain.Enums;

namespace VetCareBackend.Application.Validations
{
    public class VeterinarianRequestValidation : AbstractValidator<VeterinarianRequest>
    {
        public VeterinarianRequestValidation()
        {
            RuleFor(vet => vet.FirstName)
                .NotEmpty().WithMessage("First name is required.")
                .MinimumLength(3).WithMessage("First name must be at least 3 characters long.")
                .MaximumLength(15).WithMessage("First name cannot exceed 15 characters.");
            RuleFor(vet => vet.LastName)
                .NotEmpty().WithMessage("Last name is required.")
                .MinimumLength(3).WithMessage("Last name must be at least 3 characters long.")
                .MaximumLength(15).WithMessage("Last name cannot exceed 15 characters.");
            RuleFor(vet => vet.Dni)
                .NotEmpty().WithMessage("DNI is required.")
                .Length(8).WithMessage("DNI must be exactly 8 characters long.")
                .Must(dni => dni.All(char.IsDigit)).WithMessage("DNI must contain only numbers.");
            RuleFor(vet => vet.Email)
                .NotEmpty().WithMessage("Email address is required.")
                .EmailAddress().WithMessage("The email address format is not valid.");
            RuleFor(vet => vet.PhoneNumber)
                .NotEmpty().WithMessage("Phone number is required.")
                .MinimumLength(9).WithMessage("Phone number must be at least 9 characters long.")
                .MaximumLength(11).WithMessage("Phone number cannot exceed 11 characters.")
                .Must(pn => pn.All(char.IsDigit)).WithMessage("Phone number must contain only numbers.");
            RuleFor(vet => vet.Enrollment)
                .NotEmpty().WithMessage("The enrollment is required.")
                .Length(4).WithMessage("Enrrolment must be exactly 4 characters long.")
                .Must(enrollment => enrollment.All(char.IsDigit)).WithMessage("Enrollment must contain only numbers.");
            RuleFor(vet => vet.Speciality)
                .NotEmpty().WithMessage("The speciality is required.")
                .IsInEnum<VeterinarianRequest, Speciality>()
                .WithMessage("Invalid veterinary speciality. Please select a valid option from the list.");
        }
    }
}