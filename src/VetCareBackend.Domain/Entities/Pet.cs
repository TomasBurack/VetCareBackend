using System;
using System.Collections.Generic;
using System.Text;
using VetCareBackend.Domain.Enums;

namespace VetCareBackend.Domain.Entities
{
    public class Pet : BaseEntity
    {
        public string Name { get; set; } = string.Empty;
        public int Age { get; set; }
        public TypePet TypePet { get; set; }
        public string Breed { get; set; } = string.Empty;
        public Guid IdClient { get; set; }  
        public Client? Client { get; set; }

    }
}
