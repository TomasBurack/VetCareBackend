using System;
using System.Collections.Generic;
using System.Text;
using VetCareBackend.Application.dtos.Requests;
using VetCareBackend.Application.dtos.Responses;

namespace VetCareBackend.Application.Interfaces
{
    public interface ISysadminService
    {
        Task<UserResponse> Get(string id);
        Task Update(string id, UserRequest request);
        Task Delete(string id);
        Task<List<UserResponse>> GetAll();
    }
}
