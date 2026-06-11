using System;
using System.Collections.Generic;
using System.Text;
using VetCareBackend.Application.Interfaces;
using VetCareBackend.Domain.Entities;

namespace VetCareBackend.Infrastructure.Repository
{
    public class VeterinarianRepository : BaseRepository<Veterinarian>, IVeterinarianRepository
    { 
        public VeterinarianRepository(VetCareDbContext context) : base(context)
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
    }
}