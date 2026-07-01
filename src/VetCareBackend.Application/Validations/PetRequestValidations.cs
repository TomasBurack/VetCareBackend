using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;
using VetCareBackend.Application.dtos.Requests;

namespace VetCareBackend.Application.Validations
{
    public class PetRequestValidations : AbstractValidator<PetRequest>
    {
        public PetRequestValidations() 
        {
            RuleFor(pr => pr.Name).NotEmpty().WithMessage("This field is requiered.")
                .MinimumLength(3).WithMessage("Pet name must be at least 3 characters long.")
                .MaximumLength(20).WithMessage("Pet name cannot exceed 20 characters.")
                .When(request => !string.IsNullOrWhiteSpace(request.Name));
            RuleFor(pr => pr.Age).NotEmpty().WithMessage("This field is requiered.")
                .GreaterThanOrEqualTo(0).WithMessage("The pet age must be greater than 0.");
            RuleFor(pr => pr.typePet).NotEmpty().WithMessage("This field is requiered.");
            RuleFor(pr => pr.Breed).NotEmpty().WithMessage("This field is requiered.")
                .When(request => !string.IsNullOrWhiteSpace(request.Breed));
        }
    }
}
