using System;
using System.Collections.Generic;
using System.Text;

namespace VetCareBackend.Application.dtos.Responses
{
    public class ShiftResponse
    {
        public DateTime DateShift { get; set; }
        public string Description { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
        public string Enrrolment { get; set; } = string.Empty;
        public string VeterinarianName { get; set; } = string.Empty;
        public Guid PetId { get; set; }
        public string PetName {  get; set; } = string.Empty;

    }
}
