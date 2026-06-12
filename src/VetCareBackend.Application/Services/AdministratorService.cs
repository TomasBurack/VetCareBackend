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
        public AdministratorService(IAdministratorRepository repository,IClientRepository ClientRep, IVeterinarianRepository VetRep, IPasswordHash hash)
        {
            _AdminRep = repository;
            _ClientRep = ClientRep;
            _VetRep = VetRep;
            _hash = hash;
        }

        public UserResponse Create(SignUpRequest request)
        {
            if (_AdminRep.FindEmail(request.Email) || _ClientRep.FindEmail(request.Email) || _VetRep.FindEmail(request.Email)) {
                throw new ConflictException($"The email {request.Email} is already in use");
            } else if (_AdminRep.FindDni(request.Dni) || _ClientRep.FindDni(request.Dni) || _VetRep.FindDni(request.Dni)) {
                throw new ConflictException($"The DNI {request.Dni} is already in use");
            } else if (_AdminRep.FindPN(request.PhoneNumber) || _ClientRep.FindPN(request.PhoneNumber) || _VetRep.FindPN(request.PhoneNumber))
            {
                throw new ConflictException($"The Phone Number {request.PhoneNumber} is already in use");
            }
            //pasar las validaciones del mapper aca
            SignUpValidator validation = new SignUpValidator();
            if (!validation.Validate(request).IsValid)
            {
                throw new ValidationException(validation.Validate(request).ToString("~"));
            }
            request.Password = _hash.Hash(request.Password);
            Guid id = Guid.NewGuid();
            string dtoRole = "Administrator";
            var admin = UserMapper.ToEntity<Administrator>(request, dtoRole, id);
            _AdminRep.Add(admin);
            return UserMapper.ToDto<UserResponse>(admin);
        }

        public UserResponse Get(string id)
        {
            bool isGuid = Guid.TryParse(id, out Guid Id);
            if (!isGuid)
            {
                throw new NotFoundException("Invalid ID format");
            }
            var admin = _AdminRep.Get(Id);
            if (admin == null)
            {
                throw new NotFoundException("Administrator not found");
            }
            return UserMapper.ToDto<UserResponse>(admin);
        }

        public void Update(string id, UserRequest request)
        {
            bool isGuid = Guid.TryParse(id, out Guid Id);
            if (!isGuid)
            {
                throw new NotFoundException("Invalid ID format");
            }
            if (_AdminRep.FindEmail(request.Email) || _ClientRep.FindEmail(request.Email) || _VetRep.FindEmail(request.Email))
            {
                throw new ConflictException($"The email {request.Email} is already in use");
            }
            else if (_AdminRep.FindDni(request.Dni) || _ClientRep.FindDni(request.Dni) || _VetRep.FindDni(request.Dni))
            {
                throw new ConflictException($"The DNI {request.Dni} is already in use");
            }
            else if (_AdminRep.FindPN(request.PhoneNumber) || _ClientRep.FindPN(request.PhoneNumber) || _VetRep.FindPN(request.PhoneNumber))
            {
                throw new ConflictException($"The Phone Number {request.PhoneNumber} is already in use");
            }
            var admin = _AdminRep.Get(Id);
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
            _AdminRep.Update(admin);
        }

        public void Delete(string id)
        {
            bool isGuid = Guid.TryParse(id, out Guid Id);
            if (!isGuid)
            {
                throw new NotFoundException("Invalid ID format");
            }

            _AdminRep.Delete(Id);
        }

        public List<UserResponse> GetAll()
        {
            var list = _AdminRep.GetAll();
            return list.Select(adm => UserMapper.ToDto<UserResponse>(adm)).ToList();
        }
    }
}
