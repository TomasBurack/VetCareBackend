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
        void UpdateStatus(Guid id, ShiftStatusRequest request);
        void Delete(Guid id);

    }
}
