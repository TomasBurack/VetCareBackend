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
    }
}
