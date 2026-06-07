using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;
using VetCareBackend.Application.dtos.Requests;
using VetCareBackend.Application.dtos.Responses;
using VetCareBackend.Domain.Entities;

namespace VetCareBackend.Application.Mapper
{
    public static class ShiftMapper
    {
        public static ShiftResponse ToShiftResponse(this Shift shift)
        {
            return new ShiftResponse
            {
                ShiftId = shift.Id,
                DateShift = shift.DateShift,
                Enrollment = shift.Enrollment,
                veterinarian = shift.Veterinarian,
                PetId = shift.PetId,
                pet = shift.Pet,
                Description = shift.Description,
            };
        }

        public static Shift ToShift(this ShiftRequest shiftReq) 
        {
            return new Shift
            {
                Id = Guid.NewGuid(),
                DateShift = shiftReq.DateShift,
                Enrollment = shiftReq.Enrollment,
                PetId = shiftReq.PetId,
                Description = shiftReq.Description,
            };
        }
    }
}
