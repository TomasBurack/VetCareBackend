using System;
using System.Collections.Generic;
using System.Text;
using VetCareBackend.Domain.Entities;
using VetCareBackend.Application.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace VetCareBackend.Infrastructure.Repository
{
    public class AdministratorRepository : BaseRepository<Administrator>, IAdministratorRepository
    {
        public AdministratorRepository(VetCareDbContext context) : base(context)
        {
        }

        public async Task<bool> FindEmail(string email)
        {
            return await _dbSet.AnyAsync(x => x.Email == email && !x.IsDeleted);
        }

        public async Task<bool> FindDni(string dni)
        {
            return await _dbSet.AnyAsync(x => x.Dni == dni && !x.IsDeleted);
        }

        public async Task<bool> FindPN(string pn)
        {
            return await _dbSet.AnyAsync(x => x.PhoneNumber == pn && !x.IsDeleted);
        }
    }
}
