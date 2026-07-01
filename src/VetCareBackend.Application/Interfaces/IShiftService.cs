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
        Task<List<ShiftResponse>> GetAllAdmin(DateTimeOffset? date, Status? status, string? enrollment);
        Task<List<ShiftResponse>> GetAllVeterinarian(string sub);
        Task<List<ShiftResponse>> GetAllClient(string sub);
        Task<ShiftResponse> Create(ShiftRequest shiftReq);
        Task UpdateStatus(Guid id, ShiftStatusRequest request);
        Task Delete(Guid id);
    }
}
