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

        public UserResponse Create(SignUpRequest request)
        {
            if (_AdminRep.FindEmail(request.Email) || _repository.FindEmail(request.Email) || _VetRep.FindEmail(request.Email) || _SysadminRep.FindEmail(request.Email))
            {
                throw new ConflictException($"The email {request.Email} is already in use");
            }
            else if (_AdminRep.FindDni(request.Dni) || _repository.FindDni(request.Dni) || _VetRep.FindDni(request.Dni) || _SysadminRep.FindDni(request.Dni))
            {
                throw new ConflictException($"The DNI {request.Dni} is already in use");
            }
            else if (_AdminRep.FindPN(request.PhoneNumber) || _repository.FindPN(request.PhoneNumber) || _VetRep.FindPN(request.PhoneNumber) || _SysadminRep.FindPN(request.PhoneNumber))
            {
                throw new ConflictException($"The Phone Number {request.PhoneNumber} is already in use");
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
            _repository.Add(client);
            return UserMapper.ToDto<UserResponse>(client);
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

        public void Update(string Sub, UserRequest request)
        {
            bool Parse = Guid.TryParse(Sub, out Guid Id);
            if (Parse == false)
            {
                throw new ValidationException("The ID sent is invalid");
            }
            if (_AdminRep.FindEmail(request.Email) || _repository.FindEmail(request.Email) || _VetRep.FindEmail(request.Email) || _SysadminRep.FindEmail(request.Email))
            {
                throw new ConflictException($"The email {request.Email} is already in use");
            }
            else if (_AdminRep.FindDni(request.Dni) || _repository.FindDni(request.Dni) || _VetRep.FindDni(request.Dni) || _SysadminRep.FindDni(request.Dni))
            {
                throw new ConflictException($"The DNI {request.Dni} is already in use");
            }
            else if (_AdminRep.FindPN(request.PhoneNumber) || _repository.FindPN(request.PhoneNumber) || _VetRep.FindPN(request.PhoneNumber) || _SysadminRep.FindPN(request.PhoneNumber))
            {
                throw new ConflictException($"The Phone Number {request.PhoneNumber} is already in use");
            }
            var client = _repository.Get(Id);
            if (client == null)
            {
                throw new NotFoundException("The user was not found.");
            }

            UserRequestValidation validation = new UserRequestValidation();
            if (!validation.Validate(request).IsValid)
            {
                throw new ValidationException(validation.Validate(request).ToString("~"));
            }

            var UpdClient = UserMapper.ToEntityUpdate<Client>(client, request);

            _repository.Update(UpdClient);
        }

        public List<UserResponse> GetAll()
        {
            var list = _repository.GetAll();
            return list.Select(client => UserMapper.ToDto<UserResponse>(client)).ToList();
        }
    }
}
