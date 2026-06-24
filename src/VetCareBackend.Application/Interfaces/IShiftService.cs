using System;
using System.Collections.Generic;
using System.Text;
using VetCareBackend.Application.dtos.Requests;
using VetCareBackend.Application.dtos.Responses;

namespace VetCareBackend.Application.Interfaces
{
    public interface IShiftService
    {
        Task<List<ShiftResponse>> GetAll();
        Task<ShiftResponse> Create(ShiftRequest shiftReq);
        Task UpdateStatus(Guid id, ShiftStatusRequest request);
        Task Delete(Guid id);
    }
}
