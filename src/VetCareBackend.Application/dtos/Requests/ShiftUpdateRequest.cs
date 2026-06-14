using System;
using VetCareBackend.Domain.Enums;

namespace VetCareBackend.Application.dtos.Requests
{
    public class ShiftStatusRequest
    {
        public Status Status { get; set; }
    }
}