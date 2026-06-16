using System;
using System.Collections.Generic;
using System.Text;
using VetCareBackend.Domain.Entities;
using VetCareBackend.Application.Interfaces;

namespace VetCareBackend.Infrastructure.Repository
{
    public class AdministratorRepository : BaseRepository<Administrator>, IAdministratorRepository
    {
        public AdministratorRepository(VetCareDbContext context) : base(context)
        {
        }
        public bool FindEmail(string email)
        {
            bool value = _dbSet.Any(x => x.Email == email && !x.IsDeleted);
            return value;
        }
        public bool FindDni(string dni)
        {
            bool value = _dbSet.Any(x => x.Dni == dni && !x.IsDeleted);
            return value;
        }

        public bool FindPN(string pn)
        {
            bool value = _dbSet.Any(x => x.PhoneNumber == pn && !x.IsDeleted);
            return value;
        }

        public Administrator? GetByEmail(string email)
        {
            return _dbSet.FirstOrDefault(a => a.Email == email && !a.IsDeleted);
        }
    }
}
