using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;
using VetCareBackend.Application.dtos.Requests;
using VetCareBackend.Application.dtos.Responses;
using VetCareBackend.Application.Exceptions;
using VetCareBackend.Application.Interfaces;
using VetCareBackend.Application.Mapper;
using VetCareBackend.Application.Validations;
using VetCareBackend.Domain.Entities;


namespace VetCareBackend.Application.Services
{
    public class ClientService : IClientService
    {
        private readonly IClientRepository _repository;
        private readonly IAdministratorRepository _AdminRep;
        private readonly IVeterinarianRepository _VetRep;
        private readonly IPasswordHash _hash;
        private readonly ISysadminRepository _SysadminRep;
        public ClientService(IAdministratorRepository AdminRep, IClientRepository repository, IVeterinarianRepository VetRep, IPasswordHash hash, ISysadminRepository sysadmin)
        {
            _SysadminRep = sysadmin;
            _AdminRep = AdminRep;
            _repository = repository;
            _VetRep = VetRep;
            _hash = hash;
        }

        public async Task<UserResponse> Create(SignUpRequest request)
        {
            if (await _AdminRep.FindEmail(request.Email) || await _repository.FindEmail(request.Email) || await _VetRep.FindEmail(request.Email) || await _SysadminRep.FindEmail(request.Email))
            {
                throw new ConflictException($"The email {request.Email} is already in use");
            }
            request.Password = _hash.Hash(request.Password);
            Guid id = Guid.NewGuid();
            string dtoRole = "Client";
            SignUpValidator validation = new SignUpValidator();
            if (!validation.Validate(request).IsValid)
            {
                throw new ValidationException(validation.Validate(request).ToString("~"));
            }
            var client = UserMapper.ToEntity<Client>(request, dtoRole, id);
            await _repository.Add(client);
            return UserMapper.ToDto<UserResponse>(client);
        }

        public async Task Delete(string Sub)
        {
            bool Parse = Guid.TryParse(Sub, out Guid Id);
            if (Parse == false)
            {
                throw new ValidationException("The ID sent is invalid");
            }
            await _repository.Delete(Id);
        }

        public async Task<ClientResponse> Get(string Sub)
        {
            bool Parse = Guid.TryParse(Sub, out Guid Id);
            if (Parse == false)
            {
                throw new ValidationException("The ID sent is invalid");
            }
            var client = await _repository.Get(Id);
            if (client == null)
            {
                throw new NotFoundException("The user was not found.");
            }
            return UserMapper.ToDto<ClientResponse>(client);
        }

        public async Task Update(string Sub, UserRequest request)
        {
            bool Parse = Guid.TryParse(Sub, out Guid Id);
            if (Parse == false)
            {
                throw new ValidationException("The ID sent is invalid");
            }

            var client = await _repository.Get(Id);
            if (client == null)
            {
                throw new NotFoundException("The user was not found.");
            }

            bool emailChanged = !string.IsNullOrWhiteSpace(request.Email) && request.Email != client.Email;
            if (emailChanged && (await _AdminRep.FindEmail(request.Email) || await _repository.FindEmail(request.Email) || await _VetRep.FindEmail(request.Email) || await _SysadminRep.FindEmail(request.Email)))
            {
                throw new ConflictException($"The email {request.Email} is already in use");
            }

            UserRequestValidation validation = new UserRequestValidation();
            if (!validation.Validate(request).IsValid)
            {
                throw new ValidationException(validation.Validate(request).ToString("~"));
            }

            var UpdClient = UserMapper.ToEntityUpdate<Client>(client, request);

            await _repository.Update(UpdClient);
        }

        public async Task<List<UserResponse>> GetAll()
        {
            var list = await _repository.GetAll();
            return list.Select(client => UserMapper.ToDto<UserResponse>(client)).ToList();
        }
    }
}
