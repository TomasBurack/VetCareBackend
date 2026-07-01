using System;
using System.Collections.Generic;
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
    public class SysadminService : ISysadminService
    {
        private readonly ISysadminRepository _repository;
        private readonly IAdministratorRepository _AdminRep;
        private readonly IClientRepository _ClientRep;
        private readonly IVeterinarianRepository _VetRep;
        private readonly IPasswordHash _hash;
        public SysadminService(ISysadminRepository repository, IAdministratorRepository AdminRep, IClientRepository ClientRep, IVeterinarianRepository VetRep, IPasswordHash hash)
        {
            _AdminRep = AdminRep;
            _ClientRep = ClientRep;
            _VetRep = VetRep;
            _repository = repository;
            _hash = hash;
        }

        /*
        public async Task<UserResponse> Create(SignUpRequest request)
        {
            if (await _AdminRep.FindEmail(request.Email) || await _ClientRep.FindEmail(request.Email) || await _VetRep.FindEmail(request.Email) || await _repository.FindEmail(request.Email))
            {
                throw new ConflictException($"The email {request.Email} is already in use");
            }
            else if (await _AdminRep.FindDni(request.Dni) || await _ClientRep.FindDni(request.Dni) || await _VetRep.FindDni(request.Dni) || await _repository.FindDni(request.Dni))
            {
                throw new ConflictException($"The DNI {request.Dni} is already in use");
            }
            else if (await _AdminRep.FindPN(request.PhoneNumber) || await _ClientRep.FindPN(request.PhoneNumber) || await _VetRep.FindPN(request.PhoneNumber) || await _repository.FindPN(request.PhoneNumber))
            {
                throw new ConflictException($"The Phone Number {request.PhoneNumber} is already in use");
            }

            SignUpValidator validation = new SignUpValidator();
            if (!validation.Validate(request).IsValid)
            {
                throw new ValidationException(validation.Validate(request).ToString("~"));
            }
            request.Password = _hash.Hash(request.Password);
            Guid id = Guid.NewGuid();
            string dtoRole = "SysAdmin";
            var sysadmin = UserMapper.ToEntity<Sysadmin>(request, dtoRole, id);
            await _repository.Add(sysadmin);
            return UserMapper.ToDto<UserResponse>(sysadmin);
        }*/

        public async Task<UserResponse> Get(string id)
        {
            bool isGuid = Guid.TryParse(id, out Guid Id);
            if (!isGuid)
            {
                throw new ValidationException("Invalid ID format");
            }
            var Sysadmin = await _repository.Get(Id);
            if (Sysadmin == null)
            {
                throw new NotFoundException("Sysadmin not found");
            }
            return UserMapper.ToDto<UserResponse>(Sysadmin);
        }

        public async Task Update(string id, UserRequest request)
        {
            bool isGuid = Guid.TryParse(id, out Guid Id);
            if (!isGuid)
            {
                throw new ValidationException("Invalid ID format");
            }

            var Sysadmin = await _repository.Get(Id);
            if (Sysadmin == null)
            {
                throw new NotFoundException("Sysadmin not found");
            }

            bool emailChanged = !string.IsNullOrWhiteSpace(request.Email) && request.Email != Sysadmin.Email;
            if (emailChanged && (await _repository.FindEmail(request.Email) || await _AdminRep.FindEmail(request.Email) || await _ClientRep.FindEmail(request.Email) || await _VetRep.FindEmail(request.Email)))
            {
                throw new ConflictException($"The email {request.Email} is already in use");
            }

            UserRequestValidation validation = new UserRequestValidation();
            if (!validation.Validate(request).IsValid)
            {
                throw new ValidationException(validation.Validate(request).ToString("~"));
            }
            Sysadmin = UserMapper.ToEntityUpdate<Sysadmin>(Sysadmin, request);
            await _repository.Update(Sysadmin);
        }

        public async Task Delete(string id)
        {
            bool isGuid = Guid.TryParse(id, out Guid Id);
            if (!isGuid)
            {
                throw new ValidationException("Invalid ID format");
            }

            var Sysadmin = await _repository.Get(Id);
            if (Sysadmin == null)
            {
                throw new NotFoundException("Sysadmin not found");
            }

            await _repository.Delete(Id);
        }

        public async Task<List<UserResponse>> GetAll()
        {
            var list = await _repository.GetAll();
            return list.Select(sysadm => UserMapper.ToDto<UserResponse>(sysadm)).ToList();
        }
    }
}
