using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using VetCareBackend.Application.dtos.Requests;
using VetCareBackend.Application.dtos.Responses;
using VetCareBackend.Application.Validations;
using VetCareBackend.Domain.Entities;
using VetCareBackend.Domain.Enums;

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
                Enrollment = request.Enrollment,
                Dni=request.Dni, 
                FirstName = request.FirstName,
                LastName= request.LastName,
                Email= request.Email,
                Password= request.Password,
                PhoneNumber= request.PhoneNumber,
                Role = Role.Veterinarian,
                Speciality= request.Speciality,
            };
        }






    }

}
