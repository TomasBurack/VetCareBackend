using System;
using System.Collections.Generic;
using System.Text;
using VetCareBackend.Application.Interfaces;
using VetCareBackend.Domain.Entities;

namespace VetCareBackend.Application.Infrastructure
{
    public interface IPetRepository : IBaseRepository<Pet>
    {

    }
}
