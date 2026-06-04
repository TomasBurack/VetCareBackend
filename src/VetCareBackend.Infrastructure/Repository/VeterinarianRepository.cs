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
    }
}