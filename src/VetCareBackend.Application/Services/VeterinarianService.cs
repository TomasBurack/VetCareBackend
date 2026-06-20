using FluentValidation;
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
using VetCareBackend.Domain.Enums;
using ValidationException = VetCareBackend.Application.Exceptions.ValidationException;

namespace VetCareBackend.Application.Services
{
    public class VeterinarianService : IVeterinarianService
    {
        private readonly IVeterinarianRepository _repository;
        private readonly IPasswordHash _hash;
        private readonly IClientRepository _ClientRep;
        private readonly IAdministratorRepository _AdminRep;
        private readonly ISysadminRepository _SysadminRep;
        public VeterinarianService (IVeterinarianRepository repository, IPasswordHash hash, IClientRepository ClientRep, IAdministratorRepository AdminRep, ISysadminRepository SysadminRep)
        {
            _ClientRep = ClientRep;
            _AdminRep = AdminRep;
            _SysadminRep = SysadminRep;
            _repository = repository;
            _hash = hash;
        }

        public VeterinarianResponse Create(VeterinarianRequest request)
        {
            if (_AdminRep.FindEmail(request.Email) || _repository.FindEmail(request.Email) || _repository.FindEmail(request.Email) || _SysadminRep.FindEmail(request.Email))
            {
                throw new ConflictException($"The email {request.Email} is already in use");
            }
            VeterinarianRequestValidation validation = new VeterinarianRequestValidation();
            if (!validation.Validate(request).IsValid)
            {
                throw new ValidationException(validation.Validate(request).ToString("~"));
            }

            request.Password = _hash.Hash(request.Password);
            var veterinarian = request.ToVeterinarian();
            _repository.Add(veterinarian);
            return veterinarian.ToVeterinarianResponse();

        }
        public List<VeterinarianResponse> GetAll() {
           return _repository.GetAll().Select( v => v.ToVeterinarianResponse()).ToList();

        }
        public VeterinarianResponse GetById(string Sub) {
            bool parse = Guid.TryParse( Sub, out Guid id);
            if (!parse)
            {
                throw new ValidationException("The ID sent is invalid");
            }
            var veterinarian = _repository.Get(id);
            if (veterinarian == null)
            {
                throw new Exception("The veterinarian was not found");
            }
               
            return veterinarian.ToVeterinarianResponse();
        }
        public void Update(string Sub, VeterinarianUpdateRequest request)
        {
            bool parse = Guid.TryParse(Sub, out Guid id);
            if (!parse)
            {
                throw new ValidationException("The ID sent is invalid");
            }
            var veterinarian = _repository.Get(id);
            if (veterinarian == null)
            {
                throw new Exception("The veterinarian was not found");
            }

            VeterinarianUpdateRequestValidation validation = new VeterinarianUpdateRequestValidation();
            if (!validation.Validate(request).IsValid)
            {
                throw new ValidationException(validation.Validate(request).ToString("~"));
            }
            var UpdVet = VeterinarianMapper.ToEntityUpdate(veterinarian, request);

            _repository.Update(UpdVet);

        }
        public void Delete(string Sub)
        {
            bool parse = Guid.TryParse(Sub, out Guid id);
            if (!parse)
            {
                throw new ValidationException("The ID sent is invalid");
            }
            var veterinarian = _repository.Get(id);
            if (veterinarian == null)
            {
                throw new Exception("The veterinarian was not found");
            }
            _repository.Delete(id);

        }

        public VeterinarianResponse GetByEnrollment(string enrollment)
        {
            var vet = _repository.GetAll()
                .FirstOrDefault(v => v.Enrollment == enrollment);

            if (vet == null)
                throw new NotFoundException("The veterinarian was not found");

            return vet.ToVeterinarianResponse();
        }
    }
}
