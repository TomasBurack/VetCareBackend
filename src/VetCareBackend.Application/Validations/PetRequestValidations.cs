using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;
using VetCareBackend.Application.dtos.Requests;
using VetCareBackend.Domain.Enums;

namespace VetCareBackend.Application.Validations
{
    public class PetRequestValidations : AbstractValidator<PetRequest>
    {
        public PetRequestValidations()
        {
            RuleFor(pr => pr.Name).NotEmpty().WithMessage("Pet Name is required.")
                .MinimumLength(3).WithMessage("Pet name must be at least 3 characters long.")
                .MaximumLength(20).WithMessage("Pet name cannot exceed 20 characters.")
                .When(request => !string.IsNullOrWhiteSpace(request.Name));
            RuleFor(pr => pr.Age).NotEmpty().WithMessage("Pet age is required.")
                .GreaterThanOrEqualTo(0).WithMessage("The pet age must be positive.")
                .LessThanOrEqualTo(100).WithMessage("The pet age cannot exceed 100 years.");
            RuleFor(pr => pr.typePet)
                .IsInEnum<PetRequest, TypePet>()
                .WithMessage("Invalid pet type. Please select a valid option from the list.");
            RuleFor(pr => pr.Breed).NotEmpty().WithMessage("Pet breed is required.");
        }
    }
}
