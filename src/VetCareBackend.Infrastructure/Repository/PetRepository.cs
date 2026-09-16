using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
using VetCareBackend.Application.Infrastructure;
using VetCareBackend.Domain.Entities;

namespace VetCareBackend.Infrastructure.Repository
{
    public class PetRepository : BaseRepository<Pet>, IPetRepository
    {
        public PetRepository(VetCareDbContext context) : base(context)
        {

        }

        public async Task<List<Pet>> GetAllWithClient()
        {
            return await _dbSet.Include(p => p.Client).Where(p => !p.IsDeleted).ToListAsync();
        }

    }
}
