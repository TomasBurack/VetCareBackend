using System;
using System.Collections.Generic;
using System.Text;
using VetCareBackend.Application.dtos.Requests;
using VetCareBackend.Application.dtos.Responses;
using VetCareBackend.Application.Interfaces;
using VetCareBackend.Application.Mapper;

namespace VetCareBackend.Application.Services
{
    public class AdministratorService : IAdministratorService
    {
        public VeterinarianResponse CreateVet(VeterinarianRequest dto)
        {
            var veterinarian = VeterinarianMapper.ToVeterinarian(dto);
        }
    }
}
