using System;
using System.Collections.Generic;
using System.Text;
using VetCareBackend.Application.dtos.Requests;
using VetCareBackend.Application.dtos.Responses;

namespace VetCareBackend.Application.Interfaces
{
    public interface IAdministratorService
    {
        UserResponse Create(SignUpRequest request);

        void Update(string sub, UserRequest request);

        void Delete(string Sub);

        UserResponse Get(string Sub);

        List<UserResponse> GetAll();

        void AddRoleToUser(string id, string RoleName, string enrollment);
    }
}
