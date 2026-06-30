using System;
using System.Collections.Generic;
using System.Text;

namespace VetCareBackend.Application.dtos.Requests
{
    public class ShiftRequest
    {
        public DateTimeOffset DateShift { get; set; }
        public string Description { get; set; } = string.Empty;
        public Guid PetId { get; set; }
        public string Enrollment { get; set; } = string.Empty;
    
        
    }
}
