using System;
using System.Collections.Generic;
using System.Text;

namespace VetCareBackend.Application.dtos.Requests
{
<<<<<<< HEAD
    public class ShiftRequest 
    {
        public DateTime DateShift { get; set; }
        public string Description { get; set; } = string.Empty;
        public Guid PetId { get; set; }
        public string Enrrolment { get; set; } = string.Empty;
        
=======
    public class ShiftRequest
    {
        public DateTime DateShift { get; set; }
        public string Enrollment { get; set; } = string.Empty;
        public Guid PetId { get; set; }
        public string Description { get; set; } = string.Empty;
>>>>>>> 69cf1a12a291acc29e55816a3e4e48495159a2b7
    }
}
