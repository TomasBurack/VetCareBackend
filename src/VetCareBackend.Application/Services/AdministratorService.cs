using System;
using System.Collections.Generic;
using System.Text;
using VetCareBackend.Application.dtos.Requests;
using VetCareBackend.Application.dtos.Responses;
using VetCareBackend.Application.Exceptions;
using VetCareBackend.Application.Interfaces;
using VetCareBackend.Application.Mapper;
using VetCareBackend.Domain.Entities;


namespace VetCareBackend.Application.Services
{
    public class AdministratorService : IAdministratorService
    {
        private readonly IAdministratorRepository _repository;
        private readonly IPasswordHash _hash;
        public AdministratorService(IAdministratorRepository repository, IPasswordHash hash)
        {
            _repository = repository;
            _hash = hash;
        }

        public UserResponse Create(SignUpRequest request)
        {
            request.Password = _hash.Hash(request.Password);
            Guid id = Guid.NewGuid();
            string dtoRole = "Administrator";
            var admin = UserMapper.ToEntity<Administrator>(request, dtoRole, id);
            _repository.Add(admin);
            return UserMapper.ToDto<UserResponse>(admin);
        }

        public UserResponse Get(string id)
        {
            bool isGuid = Guid.TryParse(id, out Guid Id);
            if (!isGuid)
            {
                throw new NotFoundException("Invalid ID format");
            }
            var admin = _repository.Get(Id);
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
            var admin = _repository.Get(Id);
            if (admin == null)
            {
                throw new NotFoundException("Administrator not found");
            }
            admin = UserMapper.ToEntityUpdate<Administrator>(admin, request);
            _repository.Update(admin);
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
    }
}
