using System;
using System.Collections.Generic;
using System.Text;
using VetCareBackend.Application.dtos.Requests;
using VetCareBackend.Application.dtos.Responses;
using VetCareBackend.Application.Exceptions;
using VetCareBackend.Application.Infrastructure;
using VetCareBackend.Application.Interfaces;
using VetCareBackend.Application.Mapper;
using VetCareBackend.Application.Validations;
using VetCareBackend.Domain.Entities;

namespace VetCareBackend.Application.Services
{
    public class PetService : IPetService
    {
        private readonly IPetRepository _petRepository;
        private readonly IClientRepository _clientRepository;
        private readonly IBreedService _breedService;

        public PetService(IPetRepository petRepo, IClientRepository clientRepo, IBreedService breedService)
        {
            _petRepository = petRepo;
            _clientRepository = clientRepo;
            _breedService = breedService;
        }

        public async Task<PetResponse> Create(PetRequest petReq, string sub)
        {
            
            bool Parse = Guid.TryParse(sub, out Guid Id);
            if (Parse == false)
            {
                throw new ValidationException("The ID sent is invalid");
            }
            var client = await _clientRepository.Get(Id);

            if(client == null) 
            {
                throw new NotFoundException($"No client was found with id '{Id}'.");
            }

            PetRequestValidations validations = new PetRequestValidations();
            if (!validations.Validate(petReq).IsValid)
            {
                throw new ValidationException(validations.Validate(petReq).ToString("~"));
            }

            await ValidateBreed(petReq);

            var newPet = petReq.ToPet(client);
            await _petRepository.Add(newPet);
            return newPet.ToPetResponse();
        }

        public async Task Delete(Guid id, string sub)
        {
            bool Parse = Guid.TryParse(sub, out Guid Id);
            if (Parse == false)
            {
                throw new ValidationException("The ID sent is invalid");
            }
            var pet = await _petRepository.Get(id);

            if (pet == null || pet.IdClient != Id)
            {
                throw new NotFoundException($"No pet was found with id '{id}'.");
            }

            await _petRepository.Delete(id);
        }

        public async Task<List<PetResponse>> GetAll(string sub)
        {
            bool Parse = Guid.TryParse(sub, out Guid Id);
            if (Parse == false)
            {
                throw new ValidationException("The ID sent is invalid");
            }
            var pets = await _petRepository.GetAll();
            return pets.Where(p => p.IdClient == Id).Select(p => p.ToPetResponse()).ToList();
        }

        public async Task<PetResponse> GetById(Guid id, string sub)
        {
            bool Parse = Guid.TryParse(sub, out Guid Id);
            if (Parse == false)
            {
                throw new ValidationException("The ID sent is invalid");
            }
            var pet = await _petRepository.Get(id);

            if (pet == null || pet.IdClient != Id)
            {
                throw new NotFoundException($"No pet was found with id '{id}'.");
            }

            return pet.ToPetResponse();
        }

        public async Task Update(PetRequest petReq, Guid id, string sub)
        {
            bool Parse = Guid.TryParse(sub, out Guid Id);
            if (Parse == false)
            {
                throw new ValidationException("The ID sent is invalid");
            }
            var petToUpdate = await _petRepository.Get(id);

            if (petToUpdate == null || petToUpdate.IdClient != Id)
            {
                throw new NotFoundException($"No pet was found with id '{id}'.");
            }

            PetRequestValidations validations = new PetRequestValidations();
            if (!validations.Validate(petReq).IsValid)
            {
                throw new ValidationException(validations.Validate(petReq).ToString("~"));
            }

            await ValidateBreed(petReq);

            await _petRepository.Update(petToUpdate.ToPetUpdate(petReq));
        }

        public async Task<List<PetAdminResponse>> GetAllAdmin()
        {
            var pets = await _petRepository.GetAllWithClient();
            return pets.Select(p => p.ToPetAdminResponse()).ToList();
        }

        public async Task UpdatePetAdmin(PetRequest petReq, Guid id)
        {
            var petToUpdate = await _petRepository.Get(id);

            if (petToUpdate == null)
            {
                throw new NotFoundException($"No pet was found with id '{id}'.");
            }

            PetRequestValidations validations = new PetRequestValidations();
            if (!validations.Validate(petReq).IsValid)
            {
                throw new ValidationException(validations.Validate(petReq).ToString("~"));
            }

            await ValidateBreed(petReq);

            await _petRepository.Update(petToUpdate.ToPetUpdate(petReq));
        }

        public async Task DeletePetAdmin(Guid id)
        {
            var pet = await _petRepository.Get(id);

            if (pet == null)
            {
                throw new NotFoundException($"No pet was found with id '{id}'.");
            }

            await _petRepository.Delete(id);
        }

        private async Task ValidateBreed(PetRequest petReq)
        {
            var availableBreeds = await _breedService.GetBreedsByTypeAsync(petReq.typePet);
            if (availableBreeds.Any() && !availableBreeds.Contains(petReq.Breed, StringComparer.OrdinalIgnoreCase))
            {
                throw new ValidationException($"'{petReq.Breed}' is not a valid breed for the selected pet type.");
            }
        }
    }
}
