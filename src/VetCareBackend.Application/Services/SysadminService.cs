using System;
using System.Collections.Generic;
using System.Text;
using VetCareBackend.Application.dtos.Requests;
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
        public SysadminService(ISysadminRepository repository ,IAdministratorRepository AdminRep, IClientRepository ClientRep, IVeterinarianRepository VetRep, IPasswordHash hash)
        {
            _AdminRep = AdminRep;
            _ClientRep = ClientRep;
            _VetRep = VetRep;
            _repository = repository;
            _hash = hash;
        }

        /*
        public UserResponse Create(SignUpRequest request)
        {
            if (_AdminRep.FindEmail(request.Email) || _ClientRep.FindEmail(request.Email) || _VetRep.FindEmail(request.Email) || _repository.FindEmail(request.Email))
            {
                throw new ConflictException($"The email {request.Email} is already in use");
            }
            else if (_AdminRep.FindDni(request.Dni) || _ClientRep.FindDni(request.Dni) || _VetRep.FindDni(request.Dni) || _repository.FindDni(request.Dni))
            {
                throw new ConflictException($"The DNI {request.Dni} is already in use");
            }
            else if (_AdminRep.FindPN(request.PhoneNumber) || _ClientRep.FindPN(request.PhoneNumber) || _VetRep.FindPN(request.PhoneNumber) || _repository.FindPN(request.PhoneNumber))
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
            _repository.Add(sysadmin);
            return UserMapper.ToDto<UserResponse>(sysadmin);
        }*/
        public UserResponse Get(string id)
        {
            bool isGuid = Guid.TryParse(id, out Guid Id);
            if (!isGuid)
            {
                throw new NotFoundException("Invalid ID format");
            }
            var Sysadmin = _repository.Get(Id);
            if (Sysadmin == null)
            {
                throw new NotFoundException("Sysadmin not found");
            }
            return UserMapper.ToDto<UserResponse>(Sysadmin);
        }

        public void Update(string id, UserRequest request)
        {
            bool isGuid = Guid.TryParse(id, out Guid Id);
            if (!isGuid)
            {
                throw new NotFoundException("Invalid ID format");
            }
            if ( _repository.FindEmail(request.Email) ||_AdminRep.FindEmail(request.Email) || _ClientRep.FindEmail(request.Email) || _VetRep.FindEmail(request.Email))
            {
                throw new ConflictException($"The email {request.Email} is already in use");
            }
            var Sysadmin = _repository.Get(Id);
            if (Sysadmin == null)
            {
                throw new NotFoundException("Sysadmin not found");
            }

            UserRequestValidation validation = new UserRequestValidation();
            if (!validation.Validate(request).IsValid)
            {
                throw new ValidationException(validation.Validate(request).ToString("~"));
            }
            Sysadmin = UserMapper.ToEntityUpdate<Sysadmin>(Sysadmin, request);
            _repository.Update(Sysadmin);
        }

        public void Delete(string id)
        {
            bool isGuid = Guid.TryParse(id, out Guid Id);
            if (!isGuid)
            {
                throw new NotFoundException("Invalid ID format");
            }

            _repository.Delete(Id);
        }

        public List<UserResponse> GetAll()
        {
            var list = _repository.GetAll();
            return list.Select(sysadm => UserMapper.ToDto<UserResponse>(sysadm)).ToList();
        }

    }
}
