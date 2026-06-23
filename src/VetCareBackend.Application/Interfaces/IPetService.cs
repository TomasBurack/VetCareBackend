using System;
using System.Collections.Generic;
using System.Text;
using VetCareBackend.Application.dtos.Requests;
using VetCareBackend.Application.dtos.Responses;

namespace VetCareBackend.Application.Interfaces
{
    public interface IPetService
    {
        Task<List<PetResponse>> GetAll();
        Task<PetResponse> GetById(Guid id);
        Task<PetResponse> Create(PetRequest petReq, string sub);
        Task Update(PetRequest petReq, Guid id);
        Task Delete(Guid id);
    }
}
