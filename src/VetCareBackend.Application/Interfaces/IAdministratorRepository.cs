using System;
using System.Collections.Generic;
using System.Text;
using VetCareBackend.Domain.Entities;

namespace VetCareBackend.Application.Interfaces
{
    public interface IAdministratorRepository : IBaseRepository<Administrator>
    {
        Task<bool> FindEmail(string email);
        Task<bool> FindDni(string dni);
        Task<bool> FindPN(string pn);
    }
}
