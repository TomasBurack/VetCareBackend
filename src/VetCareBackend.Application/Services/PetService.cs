using System;
using System.Collections.Generic;
using System.Text;
using VetCareBackend.Application.dtos.Requests;
using VetCareBackend.Application.dtos.Responses;
using VetCareBackend.Application.Exceptions;
using VetCareBackend.Application.Infrastructure;
using VetCareBackend.Application.Interfaces;
using VetCareBackend.Application.Mapper;
using VetCareBackend.Domain.Entities;

namespace VetCareBackend.Application.Services
{
    public class PetService : IPetService
    {
        private readonly IPetRepository _petRepository;
        private readonly IClientRepository _clientRepository;

        public PetService(IPetRepository petRepo, IClientRepository clientRepo)
        {
            _petRepository = petRepo;
            _clientRepository = clientRepo;
        }

        public PetResponse Create(PetRequest petReq, string sub)
        {
            bool Parse = Guid.TryParse(sub, out Guid Id);
            if (Parse == false)
            {
                throw new ValidationException("The ID sent is invalid");
            }
            var client = _clientRepository.Get(Id);
            var newPet = petReq.ToPet(client);
            _petRepository.Add(newPet);
            return newPet.ToPetResponse();
        }

        public void Delete(Guid id)
        {
            var pet = _petRepository.Get(id);

            if (pet == null) 
            {
                throw new NotFoundException($"No se encontro ninguna mascota con id '{id}'.");
            }

            _petRepository.Delete(id);
        }

        public List<PetResponse> GetAll()
        {
            return _petRepository
                .GetAll()
                .Select(p => p.ToPetResponse())
                .ToList();
        }

        public PetResponse GetById(Guid id)
        {
            var pet = _petRepository.Get(id);

            if (pet == null)
            {
                throw new NotFoundException($"No se encontro ninguna mascota con id '{id}'.");
            }

            return pet.ToPetResponse();
        }

        public void Update(PetRequest petReq, Guid id)
        {
            var petToUpdate = _petRepository.Get(id);

            if (petToUpdate == null)
            {
                throw new NotFoundException($"No se encontro ninguna mascota con id '{id}'.");
            }

            petToUpdate.Name = petReq.Name;
            petToUpdate.Age = petReq.Age;
            petToUpdate.TypePet = petReq.typePet;
            petToUpdate.Breed = petReq.Breed;
            petToUpdate.UpdateDate = petReq.UpdateDate;

            _petRepository.Update(petToUpdate);
        }
    }
}
