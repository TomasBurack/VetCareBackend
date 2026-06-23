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
        Task<UserResponse> Create(SignUpRequest request);
        Task Delete(string Sub);
        Task<ClientResponse> Get(string Sub);
        Task<List<UserResponse>> GetAll();
        Task Update(string Sub, UserRequest request);
    }
}
