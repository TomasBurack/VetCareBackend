using System;
using System.Collections.Generic;
using System.Text;
using VetCareBackend.Domain.Enums;

namespace VetCareBackend.Application.dtos.Responses
{
    public class VeterinarianResponse
    {
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Enrollment { get; set; } = string.Empty;
        public Speciality Speciality { get; set; }
    }
}
