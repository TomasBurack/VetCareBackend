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

        public async Task<PetResponse> Create(PetRequest petReq, string sub)
        {
            bool Parse = Guid.TryParse(sub, out Guid Id);
            if (Parse == false)
            {
                throw new ValidationException("The ID sent is invalid");
            }
            var client = await _clientRepository.Get(Id);
            var newPet = petReq.ToPet(client);
            await _petRepository.Add(newPet);
            return newPet.ToPetResponse();
        }

        public async Task Delete(Guid id)
        {
            var pet = await _petRepository.Get(id);

            if (pet == null)
            {
                throw new NotFoundException($"No pet was found with id '{id}'.");
            }

            await _petRepository.Delete(id);
        }

        public async Task<List<PetResponse>> GetAll()
        {
            var pets = await _petRepository.GetAll();
            return pets.Select(p => p.ToPetResponse()).ToList();
        }

        public async Task<PetResponse> GetById(Guid id)
        {
            var pet = await _petRepository.Get(id);

            if (pet == null)
            {
                throw new NotFoundException($"No pet was found with id '{id}'.");
            }

            return pet.ToPetResponse();
        }

        public async Task Update(PetRequest petReq, Guid id)
        {
            var petToUpdate = await _petRepository.Get(id);

            if (petToUpdate == null)
            {
                throw new NotFoundException($"No pet was found with id '{id}'.");
            }

            petToUpdate.Name = petReq.Name;
            petToUpdate.Age = petReq.Age;
            petToUpdate.TypePet = petReq.typePet;

            await _petRepository.Update(petToUpdate);
        }
    }
}
