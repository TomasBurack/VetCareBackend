using System;
using System.Collections.Generic;
using System.Text;
using VetCareBackend.Domain.Entities;

namespace VetCareBackend.Application.Interfaces
{
    public interface IClientRepository : IBaseRepository<Client>
    {
        Task<bool> FindEmail(string email);
        Task<bool> FindDni(string dni);
        Task<bool> FindPN(string pn);
    }
}
