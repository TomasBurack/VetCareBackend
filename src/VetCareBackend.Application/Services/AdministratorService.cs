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
    public class AdministratorService : IAdministratorService
    {
        private readonly IAdministratorRepository _AdminRep;
        private readonly IClientRepository _ClientRep;
        private readonly IVeterinarianRepository _VetRep;
        private readonly IPasswordHash _hash;
        private readonly ISysadminRepository _SysadminRep;
        public AdministratorService(IAdministratorRepository repository, IClientRepository ClientRep, IVeterinarianRepository VetRep, IPasswordHash hash, ISysadminRepository sysadmin)
        {
            _AdminRep = repository;
            _ClientRep = ClientRep;
            _VetRep = VetRep;
            _hash = hash;
            _SysadminRep = sysadmin;
        }

        public async Task<UserResponse> Create(SignUpRequest request)
        {
            if (await _AdminRep.FindEmail(request.Email) || await _ClientRep.FindEmail(request.Email) || await _VetRep.FindEmail(request.Email) || await _SysadminRep.FindEmail(request.Email))
            {
                throw new ConflictException($"The email {request.Email} is already in use");
            }
            SignUpValidator validation = new SignUpValidator();
            if (!validation.Validate(request).IsValid)
            {
                throw new ValidationException(validation.Validate(request).ToString("~"));
            }
            request.Password = _hash.Hash(request.Password);
            Guid id = Guid.NewGuid();
            string dtoRole = "Administrator";
            var admin = UserMapper.ToEntity<Administrator>(request, dtoRole, id);
            await _AdminRep.Add(admin);
            return UserMapper.ToDto<UserResponse>(admin);
        }

        public async Task<UserResponse> Get(string id)
        {
            bool isGuid = Guid.TryParse(id, out Guid Id);
            if (!isGuid)
            {
                throw new NotFoundException("Invalid ID format");
            }
            var admin = await _AdminRep.Get(Id);
            if (admin == null)
            {
                throw new NotFoundException("Administrator not found");
            }
            return UserMapper.ToDto<UserResponse>(admin);
        }

        public async Task Update(string id, UserRequest request)
        {
            bool isGuid = Guid.TryParse(id, out Guid Id);
            if (!isGuid)
            {
                throw new NotFoundException("Invalid ID format");
            }
            if (await _AdminRep.FindEmail(request.Email) || await _ClientRep.FindEmail(request.Email) || await _VetRep.FindEmail(request.Email) || await _SysadminRep.FindEmail(request.Email))
            {
                throw new ConflictException($"The email {request.Email} is already in use");
            }

            var admin = await _AdminRep.Get(Id);
            if (admin == null)
            {
                throw new NotFoundException("Administrator not found");
            }

            UserRequestValidation validation = new UserRequestValidation();
            if (!validation.Validate(request).IsValid)
            {
                throw new ValidationException(validation.Validate(request).ToString("~"));
            }
            admin = UserMapper.ToEntityUpdate<Administrator>(admin, request);
            await _AdminRep.Update(admin);
        }

        public async Task Delete(string id)
        {
            bool isGuid = Guid.TryParse(id, out Guid Id);
            if (!isGuid)
            {
                throw new NotFoundException("Invalid ID format");
            }

            await _AdminRep.Delete(Id);
        }

        public async Task<List<UserResponse>> GetAll()
        {
            var list = await _AdminRep.GetAll();
            return list.Select(adm => UserMapper.ToDto<UserResponse>(adm)).ToList();
        }
    }
}
