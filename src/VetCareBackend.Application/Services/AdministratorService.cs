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
            string hashedPassword = BCrypt.Net.BCrypt.HashPassword(request.Password);
            request.Password = hashedPassword;
            Guid id = Guid.NewGuid();
            string dtoRole = "Administrator";
            var admin = UserMapper.ToEntity<Administrator>(request, dtoRole, id);
        }
        
    }
}
