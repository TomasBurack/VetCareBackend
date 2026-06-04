using System;
using System.Collections.Generic;
using System.Text;
using VetCareBackend.Domain.Entities;
using VetCareBackend.Domain.Enums;

namespace VetCareBackend.Application.dtos.Requests
{
    public class PetRequest
    {
        public string Name { get; set; } = string.Empty;
        public int Age { get; set; }
        public TypePet typePet { get; set; }
        public string Breed { get; set; } = string.Empty;
    }
}