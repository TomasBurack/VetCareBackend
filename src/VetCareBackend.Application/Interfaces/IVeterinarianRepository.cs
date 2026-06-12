using System;
using System.Collections.Generic;
using System.Text;
using VetCareBackend.Application.dtos.Requests;
using VetCareBackend.Application.dtos.Responses;
using VetCareBackend.Domain.Entities;

namespace VetCareBackend.Application.Interfaces
{
    public interface IVeterinarianRepository : IBaseRepository<Veterinarian>
    {
        bool FindEmail(string email);
        bool FindDni(string dni);
        bool FindPN(string pn);
    }
}
