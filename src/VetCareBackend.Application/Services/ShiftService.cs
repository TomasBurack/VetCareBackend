using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using VetCareBackend.Application.dtos.Requests;
using VetCareBackend.Application.dtos.Responses;
using VetCareBackend.Application.Exceptions;
using VetCareBackend.Application.Infrastructure;
using VetCareBackend.Application.Interfaces;
using VetCareBackend.Application.Mapper;
using VetCareBackend.Application.Validations;
using VetCareBackend.Domain.Entities;
using VetCareBackend.Domain.Enums;
using ValidationException = VetCareBackend.Application.Exceptions.ValidationException;

namespace VetCareBackend.Application.Services
{
    public class ShiftService : IShiftService
    {
        private readonly IShiftRepository _shiftRepository;
        private readonly IPetRepository _petRepository;
        private readonly IVeterinarianRepository _veterinarianRepository;

        public ShiftService(IShiftRepository shiftRepository, IPetRepository petRepository, IVeterinarianRepository veterinarianRepository)
        {
            _shiftRepository = shiftRepository;
            _petRepository = petRepository;
            _veterinarianRepository = veterinarianRepository;
        }
        public async Task<List<ShiftResponse>> GetAllAdmin()
        {
            var shifts = await _shiftRepository.GetAll();

            return shifts.Select(s => s.ToShiftResponse()).ToList();
        }
        public async Task<List<ShiftResponse>> GetAllVeterinarian(string sub)
        {
            bool parse = Guid.TryParse(sub, out Guid vetId);
            if (!parse)
                throw new ValidationException("The ID sent is invalid");

            var vet = await _veterinarianRepository.Get(vetId);
            if (vet == null)
                throw new NotFoundException("The veterinarian was not found.");

            var shifts = await _shiftRepository.GetAll();
            return shifts
                .Where(s => s.Enrollment == vet.Enrollment)
                .Select(s => s.ToShiftResponse())
                .ToList();
        }

        public async Task<List<ShiftResponse>> GetAllClient(string sub)
        {
            bool parse = Guid.TryParse(sub, out Guid clientId);
            if (!parse)
                throw new ValidationException("The ID sent is invalid");

            var pets = await _petRepository.GetAll();
            var petIds = pets
                .Where(p => p.IdClient == clientId)
                .Select(p => p.Id)
                .ToList();

            var shifts = await _shiftRepository.GetAll();
            return shifts
                .Where(s => petIds.Contains(s.PetId))
                .Select(s => s.ToShiftResponse())
                .ToList();
        }


        public async Task<ShiftResponse> Create(ShiftRequest shiftReq, string sub)
        {
            ShiftRequestValidations validation = new ShiftRequestValidations();

            if (!validation.Validate(shiftReq).IsValid)
            {
                throw new ValidationException(validation.Validate(shiftReq).ToString("-"));
            }

            bool Parse = Guid.TryParse(sub, out Guid Id);
            if (Parse == false)
            {
                throw new ValidationException("The ID sent is invalid");
            }

            var vets = await _veterinarianRepository.GetAll();
            var vet = vets.FirstOrDefault(v => v.Enrollment == shiftReq.Enrollment);

            if (vet == null)
                throw new NotFoundException($"No veterinarian was found with enrollment '{shiftReq.Enrollment}'.");

            var pet = await _petRepository.Get(shiftReq.PetId);

            if (pet == null)
                throw new NotFoundException($"No pet was found with id: '{shiftReq.PetId}'");

            if(Id != pet.IdClient) 
            { 
                throw new ValidationException("The pet does not belong to the authenticated client.");
            }

        var allShifts = await _shiftRepository.GetAll();

            bool shiftTaken = allShifts.Any(s =>
                s.Enrollment == shiftReq.Enrollment
                && s.DateShift == shiftReq.DateShift
                && s.Status != Status.Canceled);

            if (shiftTaken)
                throw new ValidationException($"The veterinarian already has a shift on '{shiftReq.DateShift}'.");

            var window = TimeSpan.FromMinutes(30);
            bool shiftTooClose = allShifts.Any(s =>
                s.Enrollment == shiftReq.Enrollment
                && s.DateShift != shiftReq.DateShift
                && (s.DateShift - shiftReq.DateShift) > -window
                && (s.DateShift - shiftReq.DateShift) < window);

            if (shiftTooClose)
                throw new ValidationException(
                    $"The veterinarian already has a shift within 30 minutes of '{shiftReq.DateShift}'. " +
                    $"Please choose a time at least 30 minutes apart.");

            var newShift = shiftReq.ToShift(vet, pet);
            await _shiftRepository.Add(newShift);
            return newShift.ToShiftResponse();
        }

        public async Task UpdateStatus(Guid id, ShiftStatusRequest request)
        {
            var shift = await _shiftRepository.Get(id);
            if (shift == null)
            {
                throw new NotFoundException($"No shift was found with id '{id}'.");
            }

            ShiftStatusRequestValidation validations = new ShiftStatusRequestValidation();

            if(!validations.Validate(request).IsValid)
            {
                throw new ValidationException(validations.Validate(request).ToString("-"));
            }

            if (shift.Status != Status.Pendant)
            {
                throw new ValidationException("Only shifts with status 'Pendant' can be updated.");
            }

            shift.Status = request.Status;
            await _shiftRepository.Update(shift);
        }

        public async Task Delete(Guid id)
        {
            var shift = await _shiftRepository.Get(id);
            if (shift == null)
            {
                throw new NotFoundException($"No shift was found with id '{id}'.");
            }

            if (shift.Status != Status.Pendant)
            {
                throw new ValidationException("Only shifts with status 'Pendant' can be deleted.");
            }

            await _shiftRepository.Delete(id);
        }
    }
}
