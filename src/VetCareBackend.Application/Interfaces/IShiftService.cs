using System;
using System.Collections.Generic;
using System.Text;
using VetCareBackend.Application.dtos.Requests;
using VetCareBackend.Application.dtos.Responses;
using VetCareBackend.Domain.Enums;

namespace VetCareBackend.Application.Interfaces
{
    public interface IShiftService
    {
        Task<List<ShiftResponse>> GetAllAdmin();
        Task<List<ShiftResponse>> GetAllVeterinarian(string sub);
        Task<List<ShiftResponse>> GetAllClient(string sub);
        Task<ShiftResponse> Create(ShiftRequest shiftReq, string sub);
        Task CancelStatusClient(Guid id, string sub);
        Task UpdateStatusVeterinarian(Guid id, ShiftStatusRequest request, string sub);
        Task Delete(Guid id);
    }
}
