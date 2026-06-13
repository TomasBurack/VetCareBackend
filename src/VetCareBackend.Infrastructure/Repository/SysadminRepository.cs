using System;
using System.Collections.Generic;
using System.Text;
using VetCareBackend.Application.Interfaces;
using VetCareBackend.Domain.Entities;

namespace VetCareBackend.Infrastructure.Repository
{
     public class SysadminRepository : BaseRepository<Sysadmin>, ISysadminRepository
     {
        public SysadminRepository(VetCareDbContext context) : base(context)
        {
        }
     }
}
