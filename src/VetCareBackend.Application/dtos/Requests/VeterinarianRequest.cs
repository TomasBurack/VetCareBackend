using System;
using System.Collections.Generic;
using System.Text;
using VetCareBackend.Domain.Enums;

namespace VetCareBackend.Application.dtos.Requests
{
    public class VeterinarianRequest
    {
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string Dni { get; set; } = string.Empty; 
        public string PhoneNumber { get; set; } = string.Empty;
        public string Enrollment { get; set; } = string.Empty;
        public Speciality Speciality { get; set; }
    }
}
