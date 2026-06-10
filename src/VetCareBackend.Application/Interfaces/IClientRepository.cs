using System;
using System.Collections.Generic;
using System.Text;
using VetCareBackend.Domain.Entities;

namespace VetCareBackend.Application.Interfaces
{
    public interface IClientRepository : IBaseRepository<Client>
    {
        bool FindEmail(string email);
    }
}
