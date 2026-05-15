using System;
using System.Collections.Generic;
using System.Text;

namespace VetCareBackend.Domain.Entities
{
    public class Veterinarian : User
    {
        public string Enrollment { get; set; } = string.Empty;
        public Guid UserId { get; set; }
        public User? User { get; set; }
    }
}
