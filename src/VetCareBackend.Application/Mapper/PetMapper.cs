using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using VetCareBackend.Application.dtos.Requests;
using VetCareBackend.Application.dtos.Responses;
using VetCareBackend.Application.Validations;
using VetCareBackend.Domain.Entities;
using ValidationException = VetCareBackend.Application.Exceptions.ValidationException;

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
        public static Pet ToPet(this PetRequest petReq, Client client)
        {

            return new Pet
            {
                Id = Guid.NewGuid(),
                Client = client,
                IdClient = client.Id,
                Name = petReq.Name,
                Age = petReq.Age,
                TypePet = petReq.typePet,
                Breed = petReq.Breed
            };
        }

        public static Pet ToPetUpdate(this Pet Entity, PetRequest request)
        {
            Entity.Name = string.IsNullOrWhiteSpace(request.Name) ? Entity.Name : request.Name;
            Entity.Age = request.Age;
            Entity.TypePet = request.typePet;
            Entity.Breed = string.IsNullOrWhiteSpace(request.Breed) ? Entity.Breed : request.Breed;
            return Entity;
        }
    }
}