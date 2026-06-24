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

        public async Task<List<ShiftResponse>> GetAll()
        {
            var shifts = await _shiftRepository.GetAll();
            return shifts.Select(s => s.ToShiftResponse()).ToList();
        }

        public async Task<ShiftResponse> Create(ShiftRequest shiftReq)
        {
            var vets = await _veterinarianRepository.GetAll();
            var vet = vets.FirstOrDefault(v => v.Enrollment == shiftReq.Enrollment);

            if (vet == null)
                throw new NotFoundException($"No veterinarian was found with enrollment '{shiftReq.Enrollment}'.");

            var pet = await _petRepository.Get(shiftReq.PetId);

            if (pet == null)
                throw new NotFoundException($"No pet was found with id: '{shiftReq.PetId}'");

            var allShifts = await _shiftRepository.GetAll();
            bool shiftTaken = allShifts.Any(s => s.Enrollment == shiftReq.Enrollment
                && s.DateShift == shiftReq.DateShift);

            if (shiftTaken)
                throw new ValidationException($"The veterinarian already has a shift on '{shiftReq.DateShift}'.");

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

            await _shiftRepository.Delete(id);
        }
    }
}
