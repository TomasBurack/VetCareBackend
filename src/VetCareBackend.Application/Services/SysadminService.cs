using System;
using System.Collections.Generic;
using System.Text;
using VetCareBackend.Application.Interfaces;

namespace VetCareBackend.Application.Services
{
    public class SysadminService : ISysadminService
    {
        private readonly ISysadminRepository _repository;
        public SysadminService(ISysadminRepository repository)
        {
            _repository = repository;
        }
    }
}
