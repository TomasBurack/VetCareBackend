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
        Task<bool> FindEmail(string email);
        Task<bool> FindDni(string dni);
        Task<bool> FindPN(string pn);

        Task<bool> FindEnr(string enr);
    }
}
