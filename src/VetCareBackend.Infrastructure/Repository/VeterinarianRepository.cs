using System;
using System.Collections.Generic;
using System.Text;
using VetCareBackend.Application.Interfaces;
using VetCareBackend.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace VetCareBackend.Infrastructure.Repository
{
    public class VeterinarianRepository : BaseRepository<Veterinarian>, IVeterinarianRepository
    {
        public VeterinarianRepository(VetCareDbContext context) : base(context)
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

        public async Task<bool> FindEnr(string enr)
        {
            return await _dbSet.AnyAsync(x => x.Enrollment == enr && !x.IsDeleted);
        }
    }
}
