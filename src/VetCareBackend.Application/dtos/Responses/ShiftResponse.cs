using System;
using System.Collections.Generic;
using System.Text;
using VetCareBackend.Domain.Entities;

namespace VetCareBackend.Application.dtos.Responses
{
    public class ShiftResponse
    {
        public Guid ShiftId { get; set; }
        public DateTime DateShift {  get; set; }
        public string Enrollment { get; set; } = string.Empty;
        public Veterinarian? veterinarian { get; set; }
        public Guid PetId { get; set; }
        public Pet? pet { get; set; }
        public string Description { get; set; } = string.Empty;
    }
}
