using System;
using System.Collections.Generic;
using System.Text;
using VetCareBackend.Application.dtos.Requests;
using VetCareBackend.Application.dtos.Responses;

namespace VetCareBackend.Application.Interfaces
{
    public interface IAdministratorService
    {
        Task<UserResponse> Create(SignUpRequest request);
        Task Update(string sub, UserRequest request);
        Task Delete(string Sub);
        Task<UserResponse> Get(string Sub);
        Task<List<UserResponse>> GetAll();
    }
}
