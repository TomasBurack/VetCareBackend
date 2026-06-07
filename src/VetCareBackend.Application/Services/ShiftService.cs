using System;
using System.Collections.Generic;
using System.Text;
using VetCareBackend.Application.dtos.Requests;
using VetCareBackend.Application.dtos.Responses;
using VetCareBackend.Application.Interfaces;
using VetCareBackend.Application.Mapper;

namespace VetCareBackend.Application.Services
{
    public class ShiftService : IShiftService
    {
            
        private readonly IShiftRepository _shiftRepository;

        public ShiftService(IShiftRepository shiftRepository)
        {
            _shiftRepository = shiftRepository;
        }

        public ShiftResponse Create(ShiftRequest shiftReq, string sub)
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
