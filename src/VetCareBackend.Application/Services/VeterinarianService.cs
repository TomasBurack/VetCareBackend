using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;
using VetCareBackend.Application.dtos.Requests;
using VetCareBackend.Application.dtos.Responses;
using VetCareBackend.Application.Exceptions;
using VetCareBackend.Application.Interfaces;
using VetCareBackend.Application.Mapper;
using ValidationException = VetCareBackend.Application.Exceptions.ValidationException;

namespace VetCareBackend.Application.Services
{
    public class VeterinarianService : IVeterinarianService
    {
        private readonly IVeterinarianRepository _repository;
        private readonly IPasswordHash _hash;
        public VeterinarianService (IVeterinarianRepository repository, IPasswordHash hash)
        { 
            _repository = repository;
            _hash = hash;
        }

        public VeterinarianResponse Create(VeterinarianRequest request)
        {

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
        public void Update(string Sub, VeterinarianRequest request)
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

            veterinarian.FirstName = request.FirstName;
            veterinarian.LastName = request.LastName;
            veterinarian.PhoneNumber = request.PhoneNumber;
            veterinarian.Email = request.Email;
            veterinarian.Enrollment = request.Enrollment;
            veterinarian.Speciality = request.Speciality;

            _repository.Update(veterinarian);
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
