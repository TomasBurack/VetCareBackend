using System;
using System.Collections.Generic;
using System.Text;
using VetCareBackend.Application.Requests;
using VetCareBackend.Application.Responses;
using VetCareBackend.Domain.Entities;

namespace VetCareBackend.Application.Mapper
{
    public static class PetMapper
    {
        public static PetResponse ToPetResponse(this Pet pet)
        {
            return new PetResponse
            {
                IdPet = pet.Id,
                Name = pet.Name,
                Age = pet.Age,
                TypePet = pet.TypePet,
                Breed = pet.Breed
            };
        }
        public static Pet ToPet(this PetRequest petReq)
        {
            return new Pet
            {
                Id = Guid.NewGuid(),
                Name = petReq.Name,
                Age = petReq.Age,
                TypePet = petReq.typePet,
                Breed = petReq.Breed
            };
        }
    }
}