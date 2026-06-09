using System;
using System.Collections.Generic;
using System.Text;
using VetCareBackend.Application.dtos.Requests;
using VetCareBackend.Application.dtos.Responses;
using VetCareBackend.Application.Infrastructure;
using VetCareBackend.Application.Interfaces;
using VetCareBackend.Application.Mapper;

namespace VetCareBackend.Application.Services
{
    public class ShiftService : IShiftService
    {
            
        private readonly IShiftRepository _shiftRepository;
        private readonly IPetRepository _petRepository;
        private readonly IVeterinarianRepository _veterinarianRepository;

        public ShiftService(IShiftRepository shiftRepo, IVeterinarianRepository veterinarianRepo, IPetRepository petRepo)
        {
            _shiftRepository = shiftRepo;
            _veterinarianRepository = veterinarianRepo;
            _petRepository = petRepo;

        }

        public ShiftResponse Create(ShiftRequest shiftReq)
        {
            throw new NotImplementedException();
        }

        public void Delete(Guid id)
        {
            throw new NotImplementedException();
        }

        public List<ShiftResponse> GetAll()
        {
            throw new NotImplementedException();
        }

        public void Update(ShiftRequest shiftReq, Guid id)
        {
            throw new NotImplementedException();
        }
    }
}
