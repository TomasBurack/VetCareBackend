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
        public Guid IdBreed { get; set; }
        public Breed? Breed { get; set; }
        public Guid IdClient { get; set; }  
        public Client? Client { get; set; }

    }
}
