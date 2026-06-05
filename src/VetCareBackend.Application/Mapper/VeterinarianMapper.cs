using System;
using System.Collections.Generic;
using System.Text;
using VetCareBackend.Application.dtos.Requests;
using VetCareBackend.Domain.Entities;
using VetCareBackend.Application.dtos.Responses;
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
                Dni = veterinarian.Dni,
                Email = veterinarian.Email,
                PhoneNumber = veterinarian.PhoneNumber,
                Enrollment = veterinarian.Enrollment,
                Speciality = veterinarian.Speciality,
            };
        }
        public static Veterinarian ToVeterinarian(this VeterinarianRequest request)
        {
            return new Veterinarian
            {
                Dni = request.Dni,
                Email = request.Email,
                PhoneNumber = request.PhoneNumber,
                Enrollment = request.Enrollment,
                FirstName = request.FirstName,
                LastName= request.LastName,
                Role = Role.Veterinarian,
                Speciality= request.Speciality,
            };
        }






    }

}
