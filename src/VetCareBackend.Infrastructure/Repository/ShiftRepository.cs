using System;
using System.Collections.Generic;
using System.Text;
using VetCareBackend.Domain.Entities;

namespace VetCareBackend.Infrastructure.Repository
{
    public class ShiftRepository : BaseRepository<Shift>, IShiftRepository
    {
        public ShiftRepository(VetCareDbContext context) : base(context)
        {
        }
    } 
}
