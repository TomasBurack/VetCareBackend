using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;
using VetCareBackend.Application.dtos.Requests;
using VetCareBackend.Application.dtos.Responses;
using VetCareBackend.Domain.Entities;
using VetCareBackend.Domain.Enums;

namespace VetCareBackend.Application.Mapper
{
    public static class ShiftMapper
    {
        public static ShiftResponse ToShiftResponse(this Shift shift)
        {
            return new ShiftResponse
            {
                DateShift = shift.DateShift,
                Description = shift.Description,
                Status = shift.Status.ToString(),
                Enrollment = shift.Enrollment,
                VeterinarianName = shift.Veterinarian?.FirstName + " " + shift.Veterinarian?.LastName,
                PetId = shift.PetId,
                PetName = shift.Pet?.Name ?? string.Empty
            };
            
        }
        public static Shift ToShift(this ShiftRequest request, Veterinarian veterinarian, Pet pet)
           
        {
            return new Shift
            {
                Id = Guid.NewGuid(),
                DateShift = request.DateShift,
                Description = request.Description,
                Enrollment = veterinarian.Enrollment,
                Veterinarian = veterinarian,
                PetId = pet.Id,
                Status = Status.Pendient

            };
        
        }


    }
}
