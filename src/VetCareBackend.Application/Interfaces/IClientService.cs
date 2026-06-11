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
        UserResponse Create(SignUpRequest request);
        void Delete(string Sub);
        ClientResponse Get(string Sub);

        void Update(string Sub, UserRequest request);
    }
}
