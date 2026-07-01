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
        public VeterinarianService(IVeterinarianRepository repository, IPasswordHash hash, IClientRepository ClientRep, IAdministratorRepository AdminRep, ISysadminRepository SysadminRep)
        {
            _ClientRep = ClientRep;
            _AdminRep = AdminRep;
            _SysadminRep = SysadminRep;
            _repository = repository;
            _hash = hash;
        }

        public async Task<VeterinarianResponse> Create(VeterinarianRequest request)
        {
            if (await _AdminRep.FindEmail(request.Email) || await _repository.FindEmail(request.Email) || await _SysadminRep.FindEmail(request.Email) || await _ClientRep.FindEmail(request.Email))
            {
                throw new ConflictException($"The email {request.Email} is already in use");
            }
            bool enrollmentUsed = await _repository.FindEnr(request.Enrollment);
            if (enrollmentUsed)
            {
                throw new ConflictException($"The enrollment {request.Enrollment} is already in use");
            }
            VeterinarianRequestValidation validation = new VeterinarianRequestValidation();
            if (!validation.Validate(request).IsValid)
            {
                throw new ValidationException(validation.Validate(request).ToString("~"));
            }

            request.Password = _hash.Hash(request.Password);
            var veterinarian = request.ToVeterinarian();
            await _repository.Add(veterinarian);
            return veterinarian.ToVeterinarianResponse();
        }

        public async Task<List<VeterinarianResponse>> GetAll()
        {
            var list = await _repository.GetAll();
            return list.Select(v => v.ToVeterinarianResponse()).ToList();
        }

        public async Task<VeterinarianResponse> GetById(string Sub)
        {
            bool parse = Guid.TryParse(Sub, out Guid id);
            if (!parse)
            {
                throw new ValidationException("The ID sent is invalid");
            }
            var veterinarian = await _repository.Get(id);
            if (veterinarian == null)
            {
                throw new NotFoundException("The veterinarian was not found");
            }

            return veterinarian.ToVeterinarianResponse();
        }

        public async Task Update(string Sub, VeterinarianUpdateRequest request)
        {
            bool parse = Guid.TryParse(Sub, out Guid id);
            if (!parse)
            {
                throw new ValidationException("The ID sent is invalid");
            }
            var veterinarian = await _repository.Get(id);
            if (veterinarian == null)
            {
                throw new NotFoundException("The veterinarian was not found");
            }
            bool emailUsed = await _repository.FindEmail(request.Email) || await _AdminRep.FindEmail(request.Email) || await _SysadminRep.FindEmail(request.Email) || await _ClientRep.FindEmail(request.Email);
            if (emailUsed && request.Email != veterinarian.Email)
            {
                throw new ConflictException($"The email {request.Email} is already in use");
            }
            bool enrollmentUsed = await _repository.FindEnr(request.Enrollment);
            if (enrollmentUsed && request.Enrollment != veterinarian.Enrollment)
            {
                throw new ConflictException($"The enrollment {request.Enrollment} is already in use");
            }

            VeterinarianUpdateRequestValidation validation = new VeterinarianUpdateRequestValidation();
            if (!validation.Validate(request).IsValid)
            {
                throw new ValidationException(validation.Validate(request).ToString("~"));
            }
            var UpdVet = VeterinarianMapper.ToEntityUpdate(veterinarian, request);

            await _repository.Update(UpdVet);
        }

        public async Task Delete(string Sub)
        {
            bool parse = Guid.TryParse(Sub, out Guid id);
            if (!parse)
            {
                throw new ValidationException("The ID sent is invalid");
            }
            var veterinarian = await _repository.Get(id);
            if (veterinarian == null)
            {
                throw new NotFoundException("The veterinarian was not found");
            }
            await _repository.Delete(id);
        }

        public async Task<VeterinarianResponse> GetByEnrollment(string enrollment)
        {
            var list = await _repository.GetAll();
            var vet = list.FirstOrDefault(v => v.Enrollment == enrollment);

            if (vet == null)
                throw new NotFoundException("The veterinarian was not found");

            return vet.ToVeterinarianResponse();
        }
    }
}
