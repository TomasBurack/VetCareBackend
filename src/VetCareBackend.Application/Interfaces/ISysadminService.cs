using System;
using System.Collections.Generic;
using System.Text;
using VetCareBackend.Application.dtos.Requests;

namespace VetCareBackend.Application.Interfaces
{
    public interface ISysadminService
    {
        /*UserResponse Create(SignUpRequest request);*/
        UserResponse Get(string id);
        void Update(string id, UserRequest request);
        void Delete(string id);
        List<UserResponse> GetAll();
    }
}
