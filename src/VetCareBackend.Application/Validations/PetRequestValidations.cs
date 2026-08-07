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
            RuleFor(pr => pr.Name).NotEmpty().WithMessage("El nombre de la mascota es obligatorio.")
                .MinimumLength(3).WithMessage("El nombre de la mascota debe tener al menos 3 caracteres.")
                .MaximumLength(20).WithMessage("El nombre de la mascota no puede superar los 20 caracteres.")
                .When(request => !string.IsNullOrWhiteSpace(request.Name));
            RuleFor(pr => pr.Age).NotEmpty().WithMessage("La edad de la mascota es obligatoria.")
                .GreaterThanOrEqualTo(0).WithMessage("La edad de la mascota debe ser positiva.");
            RuleFor(pr => pr.Age)
                .LessThanOrEqualTo(30).WithMessage("La edad de un perro no puede superar los 30 años.")
                .When(pr => pr.typePet == TypePet.Canine);
            RuleFor(pr => pr.Age)
                .LessThanOrEqualTo(35).WithMessage("La edad de un gato no puede superar los 35 años.")
                .When(pr => pr.typePet == TypePet.Feline);
            RuleFor(pr => pr.Age)
                .LessThanOrEqualTo(100).WithMessage("La edad de un ave no puede superar los 100 años.")
                .When(pr => pr.typePet == TypePet.Avian);
            RuleFor(pr => pr.Age)
                .LessThanOrEqualTo(100).WithMessage("La edad de un reptil no puede superar los 100 años.")
                .When(pr => pr.typePet == TypePet.Reptile);
            RuleFor(pr => pr.typePet)
                .IsInEnum<PetRequest, TypePet>()
                .WithMessage("El tipo de mascota no es válido. Por favor, seleccioná una opción válida de la lista.");
            RuleFor(pr => pr.Breed).NotEmpty().WithMessage("La raza de la mascota es obligatoria.");
        }
    }
}
