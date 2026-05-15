using System;
using System.Collections.Generic;
using System.Text;
using VetCareBackend.Domain.Enums;

namespace VetCareBackend.Domain.Entities
{
    public class Shift : BaseEntity
    {
        public DateTime DateShift { get; set; }

        public TypeConsult TypeConsult { get; set; }

        public string Enrollment {  get; set; } = string.Empty;

        public Veterinarian? Veterinarian { get; set; }

        public Guid PetId { get; set; }

        public Pet? Pet { get; set; }

        public string Description { get; set; } = string.Empty;

        public Status Status { get; set; }



    }
}
