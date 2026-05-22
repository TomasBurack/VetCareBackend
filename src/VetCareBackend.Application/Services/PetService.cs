using System;
using System.Collections.Generic;
using System.Text;
using VetCareBackend.Application.dtos.Requests;
using VetCareBackend.Application.dtos.Responses;
using VetCareBackend.Application.Interfaces;

namespace VetCareBackend.Application.Services
{
    public class PetService : IPetService
    {
        

        public PetResponse Create(PetRequest petReq)
        {
            throw new NotImplementedException();
        }

        public void Delete(PetRequest petReq, Guid id)
        {
            throw new NotImplementedException();
        }

        public List<PetResponse> GetAll()
        {
            throw new NotImplementedException();
        }

        public PetResponse GetById(Guid id)
        {
            throw new NotImplementedException();
        }

        void IPetService.Update(PetRequest petReq, Guid id)
        {
            throw new NotImplementedException();
        }
    }
}
