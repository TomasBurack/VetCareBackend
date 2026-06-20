using System;
using System.Collections.Generic;
using System.Text;
using VetCareBackend.Application.dtos.Requests;
using VetCareBackend.Application.dtos.Responses;
using VetCareBackend.Application.Validations;
using VetCareBackend.Domain.Entities;
using VetCareBackend.Domain.Enums;
using VetCareBackend.Application.Exceptions;
using ValidationException = VetCareBackend.Application.Exceptions.ValidationException;

namespace VetCareBackend.Application.Mapper
{
    public static class VeterinarianMapper
    {
        public static VeterinarianResponse ToVeterinarianResponse(this Veterinarian veterinarian)
        {
            return new VeterinarianResponse
            {
                Id = veterinarian.Id,
                FirstName = veterinarian.FirstName,
                LastName = veterinarian.LastName,
                Dni = veterinarian.Dni,
                Email = veterinarian.Email,
                PhoneNumber = veterinarian.PhoneNumber,
                Enrollment = veterinarian.Enrollment,
                Speciality = nameof(veterinarian.Speciality)
            };
        }
        public static Veterinarian ToVeterinarian(this VeterinarianRequest request)
        {
            
            return new Veterinarian
            {
                Id = Guid.NewGuid(),
                Dni = request.Dni,
                Email = request.Email,
                PhoneNumber = request.PhoneNumber,
                Enrollment = request.Enrollment,
                FirstName = request.FirstName,
                LastName = request.LastName,
                Password = request.Password,
                Role = Role.Veterinarian,
                Speciality = request.Speciality,

            };
        }

        public static Veterinarian ToEntityUpdate(this Veterinarian Entity, VeterinarianUpdateRequest request)
        {
            Entity.FirstName = string.IsNullOrWhiteSpace(request.FirstName) ? Entity.FirstName : request.FirstName;
            Entity.LastName = string.IsNullOrWhiteSpace(request.LastName) ? Entity.LastName : request.LastName;
            Entity.Dni = string.IsNullOrWhiteSpace(request.Dni) ? Entity.Dni : request.Dni;
            Entity.Email = string.IsNullOrWhiteSpace(request.Email) ? Entity.Email : request.Email;
            Entity.PhoneNumber = string.IsNullOrWhiteSpace(request.PhoneNumber) ? Entity.PhoneNumber : request.PhoneNumber;
            Entity.Enrollment = string.IsNullOrWhiteSpace(request.Enrollment) ? Entity.Enrollment : request.Enrollment;
            Entity.Speciality = Entity.Speciality != request.Speciality ? request.Speciality : Entity.Speciality;
            return Entity;
        }






    }

}
