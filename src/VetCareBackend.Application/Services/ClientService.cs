using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;
using VetCareBackend.Application.dtos.Responses;
using VetCareBackend.Application.Exceptions;
using VetCareBackend.Application.Interfaces;
using VetCareBackend.Application.Mapper;
using VetCareBackend.Domain.Entities;


namespace VetCareBackend.Application.Services
{
    public class ClientService : IClientService
    {
        private readonly IClientRepository _repository;
        public ClientService(IClientRepository repository)
        {
            _repository = repository;
        }
        public void Delete(string Sub)
        {
            bool Parse = Guid.TryParse(Sub, out Guid Id);
            if (Parse == false)
            {
                throw new ValidationException("The ID sent is invalid");
            }
            _repository.Delete(Id);
        }

        public ClientResponse Get(string Sub)
        {
            bool Parse = Guid.TryParse(Sub, out Guid Id);
            if (Parse == false)
            {
                throw new ValidationException("The ID sent is invalid");
            }
            var client = _repository.Get(Id);
            if (client == null)
            {
                throw new NotFoundException("The user was not found.");
            }
            return UserMapper.ToDto<ClientResponse>(client);
        }

        public void Update(string Sub)
        {
            bool Parse = Guid.TryParse(Sub, out Guid Id);
            if (Parse == false)
            {
                throw new ValidationException("The ID sent is invalid");
            }
            var client = _repository.Get(Id);
            if (client == null)
            {
                throw new NotFoundException("The user was not found.");
            }
            _repository.Update(client);
        }
    }
}
