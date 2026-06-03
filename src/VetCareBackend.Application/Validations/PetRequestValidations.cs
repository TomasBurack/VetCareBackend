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
            RuleFor(pr => pr.Name).NotEmpty().MinimumLength(3).MaximumLength(20);
            RuleFor(pr => pr.Age).NotEmpty().GreaterThanOrEqualTo(0);
            RuleFor(pr => pr.typePet).NotEmpty();
            RuleFor(pr => pr.Breed).NotEmpty();
        }
    }
}
