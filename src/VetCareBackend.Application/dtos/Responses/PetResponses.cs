using System;
using System.Collections.Generic;
using System.Text;
using VetCareBackend.Domain.Entities;
using VetCareBackend.Domain.Enums;

namespace VetCareBackend.Application.dtos.Responses
{
    public class PetResponse
    {
        public Guid IdPet { get; set; }
        public string Name { get; set; } = string.Empty;
        public int Age { get; set; }
        public TypePet TypePet { get; set; }
        public string Breed { get; set; } = string.Empty;
        public DateTime UpdateDate { get; set; }
    }
}