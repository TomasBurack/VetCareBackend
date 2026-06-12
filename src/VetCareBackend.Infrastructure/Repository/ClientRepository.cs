using System;
using System.Collections.Generic;
using System.Text;
using VetCareBackend.Application.Interfaces;
using VetCareBackend.Domain.Entities;

namespace VetCareBackend.Infrastructure.Repository
{
    public class ClientRepository : BaseRepository<Client>, IClientRepository
    {
        public ClientRepository(VetCareDbContext context) : base(context)
        {
        }

        public bool FindEmail(string email) 
        {
            _dbSet.Any(x=> x.Email == email && !x.IsDeleted);
        }
    }
}
