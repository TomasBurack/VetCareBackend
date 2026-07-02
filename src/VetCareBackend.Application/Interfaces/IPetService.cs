using System;
using System.Collections.Generic;
using System.Text;
using VetCareBackend.Application.dtos.Requests;
using VetCareBackend.Application.dtos.Responses;

namespace VetCareBackend.Application.Interfaces
{
    public interface IPetService
    {
        Task<List<PetResponse>> GetAll(string sub);
        Task<PetResponse> GetById(Guid id, string sub);
        Task<PetResponse> Create(PetRequest petReq, string sub);
        Task Update(PetRequest petReq, Guid id, string sub);
        Task Delete(Guid id, string sub);
    }
}
