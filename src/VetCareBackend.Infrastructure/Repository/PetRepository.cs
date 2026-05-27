using System;
using System.Collections.Generic;
using System.Text;
using VetCareBackend.Application.Infrastructure;
using VetCareBackend.Domain.Entities;

namespace VetCareBackend.Infrastructure.Repository
{
    public class PetRepository : BaseRepository<Pet>, IPetRepository
    {
        public PetRepository(VetCareDbContext context) : base(context)
        {
            
        }

    }
}
