using System;
using System.Collections.Generic;
using System.Text;
using VetCareBackend.Domain.Enums;

namespace VetCareBackend.Domain.Entities
{
    public class Veterinarian : User
    {
        public string Enrollment { get; set; } = string.Empty;
        public Speciality Speciality {  get; set; }
        
    }
}
