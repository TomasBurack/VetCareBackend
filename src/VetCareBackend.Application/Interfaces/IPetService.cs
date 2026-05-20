using System;
using System.Collections.Generic;
using System.Text;
using VetCareBackend.Application.Requests;
using VetCareBackend.Application.Responses;

namespace VetCareBackend.Application.Interfaces
{
    public interface IPetService
    {
        List<PetResponse> GetAll();
        PetResponse GetById(Guid id);
        PetResponse Create(PetRequest petReq);
        bool Update(PetRequest petReq, Guid id);
    }
}