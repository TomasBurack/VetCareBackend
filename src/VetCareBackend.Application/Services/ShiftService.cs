<<<<<<< HEAD
﻿using System;
using System.Collections.Generic;
using System.Text;

namespace VetCareBackend.Application.Services
{
    public class ShiftService 
    {
    }
}
=======
﻿using FluentValidation;
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


        public ShiftService( IShiftRepository shiftRepository, IPetRepository petRepository, IVeterinarianRepository veterinarianRepository)
        {
            _shiftRepository = shiftRepository;
            _petRepository = petRepository;
            _veterinarianRepository = veterinarianRepository;
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
           var vet = _veterinarianRepository.GetAll()
                .FirstOrDefault(v => v.Enrollment == shiftReq.Enrollment);

            if(vet == null) 
                throw new NotFoundException($"No veterinarian was found with enrollment '{shiftReq.Enrollment}'.");

            var pet = _petRepository.Get(shiftReq.PetId);

            if(pet == null)
                throw new NotFoundException($"No pet was found with id: '{shiftReq.PetId}'");

            bool shiftTaken = _shiftRepository.GetAll()
                .Any(s => s.Enrollment == shiftReq.Enrollment 
                && s.DateShift == shiftReq.DateShift);

            if (shiftTaken)
                throw new ValidationException($"The veterinarian already has a shift on '{shiftReq.DateShift}'.");

            var newShift = shiftReq.ToShift(vet, pet);
            _shiftRepository.Add(newShift);
            return newShift.ToShiftResponse();
        }

        public void Update(ShiftStatusRequest request, Guid id)
        {
            var shift = _shiftRepository.Get(id);
            if (shift == null)
            {
                throw new NotFoundException($"No shift was found with id '{id}'.");
            }

            shift.Status = request.Status;
            _shiftRepository.Update(shift);
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
>>>>>>> 69cf1a12a291acc29e55816a3e4e48495159a2b7
