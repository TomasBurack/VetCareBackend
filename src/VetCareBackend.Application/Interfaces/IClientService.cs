using System;
using System.Collections.Generic;
using System.Text;
using VetCareBackend.Application.dtos.Requests;
using VetCareBackend.Application.dtos.Responses;
using VetCareBackend.Domain.Entities;

namespace VetCareBackend.Application.Interfaces
{
    public interface IClientService
    {
        public void Delete(string Sub);
        public ClientResponse Get(string Sub);

        public void Update(string Sub);
    }
}
