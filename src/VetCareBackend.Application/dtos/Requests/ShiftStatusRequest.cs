using System;
using System.Collections.Generic;
using System.Text;
using VetCareBackend.Domain.Enums;

namespace VetCareBackend.Application.dtos.Requests
{
    public class ShiftStatusRequest
    {
        public Status Status {  get; set; }

    }
}
