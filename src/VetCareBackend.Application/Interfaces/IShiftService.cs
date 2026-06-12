using System;
using System.Collections.Generic;
using System.Text;
using VetCareBackend.Application.dtos.Requests;
using VetCareBackend.Application.dtos.Responses;

namespace VetCareBackend.Application.Interfaces
{
    public interface IShiftService
    {
        List<ShiftResponse> GetAll();
        ShiftResponse Create(ShiftRequest shiftReq);
        void Update(ShiftRequest shiftReq, Guid id);
        void Delete(Guid id);

    }
}
