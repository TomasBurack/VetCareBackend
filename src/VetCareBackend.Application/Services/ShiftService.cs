using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VetCareBackend.Application.dtos.Requests;
using VetCareBackend.Application.dtos.Responses;
using VetCareBackend.Application.Exceptions;
using VetCareBackend.Application.Infrastructure;
using VetCareBackend.Application.Interfaces;
using VetCareBackend.Application.Mapper;
using VetCareBackend.Domain.Entities;
using VetCareBackend.Domain.Enums;

namespace VetCareBackend.Application.Services
{
    public class ShiftService : IShiftService
    {
        private readonly IShiftRepository _shiftRepository;
        private readonly IPetRepository _petRepository;
        private readonly IVeterinarianRepository _veterinarianRepository;
        private readonly IValidator<ShiftRequest> _validator;

        public ShiftService(
            IShiftRepository shiftRepository,
            IPetRepository petRepository,
            IVeterinarianRepository veterinarianRepository,
            IValidator<ShiftRequest> validator)
        {
            _shiftRepository = shiftRepository;
            _petRepository = petRepository;
            _veterinarianRepository = veterinarianRepository;
            _validator = validator;
        }

        public List<ShiftResponse> GetAll()
        {
            return _shiftRepository
                .GetAll()
                .Select(s => s.ToShiftResponse())
                .ToList();
        }

        public ShiftResponse Create(ShiftRequest shiftReq)
        {
            _validator.ValidateAndThrow(shiftReq);

            var veterinarian = _veterinarianRepository.GetAll()
                .FirstOrDefault(v => v.Enrollment == shiftReq.Enrollment);

            if (veterinarian == null)
            {
                throw new NotFoundException($"No veterinarian was found with enrollment '{shiftReq.Enrollment}'.");
            }

            var pet = _petRepository.Get(shiftReq.PetId);
            if (pet == null)
            {
                throw new NotFoundException($"No pet was found with id '{shiftReq.PetId}'.");
            }

            var newShift = new Shift
            {
                DateShift = shiftReq.DateShift,
                Enrollment = shiftReq.Enrollment,
                Veterinarian = veterinarian,
                PetId = shiftReq.PetId,
                Pet = pet,
                Description = shiftReq.Description,
                Status = Status.Pending
            };

            _shiftRepository.Add(newShift);
            return newShift.ToShiftResponse();
        }

        public void Update(ShiftRequest shiftReq, Guid id)
        {
            _validator.ValidateAndThrow(shiftReq);

            var shiftToUpdate = _shiftRepository.Get(id);
            if (shiftToUpdate == null)
            {
                throw new NotFoundException($"No shift was found with id '{id}'.");
            }

            var veterinarian = _veterinarianRepository.GetAll()
                .FirstOrDefault(v => v.Enrollment == shiftReq.Enrollment);

            if (veterinarian == null)
            {
                throw new NotFoundException($"No veterinarian was found with enrollment '{shiftReq.Enrollment}'.");
            }

            var pet = _petRepository.Get(shiftReq.PetId);
            if (pet == null)
            {
                throw new NotFoundException($"No pet was found with id '{shiftReq.PetId}'.");
            }

            shiftToUpdate.DateShift = shiftReq.DateShift;
            shiftToUpdate.Enrollment = shiftReq.Enrollment;
            shiftToUpdate.Veterinarian = veterinarian;
            shiftToUpdate.PetId = shiftReq.PetId;
            shiftToUpdate.Pet = pet;
            shiftToUpdate.Description = shiftReq.Description;

            _shiftRepository.Update(shiftToUpdate);
        }

        public void Delete(Guid id)
        {
            var shift = _shiftRepository.Get(id);
            if (shift == null)
            {
                throw new NotFoundException($"No shift was found with id '{id}'.");
            }

            _shiftRepository.Delete(id);
        }
    }
}