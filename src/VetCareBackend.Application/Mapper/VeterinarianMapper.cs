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
                
                FirstName = veterinarian.FirstName,
                LastName = veterinarian.LastName,
                Dni = veterinarian.Dni,
                Email = veterinarian.Email,
                PhoneNumber = veterinarian.PhoneNumber,
                Enrollment = veterinarian.Enrollment,
                Speciality = veterinarian.Speciality,
            };
        }
        public static Veterinarian ToVeterinarian(this VeterinarianRequest request)
        {
            VeterinarianRequestValidation validation = new VeterinarianRequestValidation();
            if (!validation.Validate(request).IsValid)
            {
                throw new ValidationException(validation.Validate(request).ToString("~"));
            }
            return new Veterinarian
            {
                Dni = request.Dni,
                Email = request.Email,
                PhoneNumber = request.PhoneNumber,
                Enrollment = request.Enrollment,
                FirstName = request.FirstName,
<<<<<<< HEAD
                LastName = request.LastName,
                Password = request.Password,
                Role = Role.Veterinarian,
                Speciality = request.Speciality,
=======
                LastName= request.LastName,
                Password= request.Password,
                Role = Role.Veterinarian,
                Speciality= request.Speciality,
>>>>>>> b3a7e67cd6be22e519066ae94dc8469e7cf22809
            };
        }






    }

}
