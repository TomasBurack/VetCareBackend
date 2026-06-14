using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;
using VetCareBackend.Application.dtos.Requests;
using VetCareBackend.Application.dtos.Responses;
using VetCareBackend.Domain.Entities;
<<<<<<< HEAD
using VetCareBackend.Domain.Enums;
=======
>>>>>>> 69cf1a12a291acc29e55816a3e4e48495159a2b7

namespace VetCareBackend.Application.Mapper
{
    public static class ShiftMapper
    {
        public static ShiftResponse ToShiftResponse(this Shift shift)
        {
            return new ShiftResponse
            {
<<<<<<< HEAD
                DateShift = shift.DateShift,
                Description = shift.Description,
                Status = shift.Status.ToString(),
                Enrrolment = shift.Enrollment,
                VeterinarianName = shift.Veterinarian?.FirstName + " " + shift.Veterinarian?.LastName,
                PetId = shift.PetId,
                PetName = shift.Pet?.Name ?? string.Empty
            };
            
        }
        public static Shift ToShift(this ShiftRequest request, Veterinarian veterinarian, Pet pet)
           
=======
                ShiftId = shift.Id,
                DateShift = shift.DateShift,
                Enrollment = shift.Enrollment,
                veterinarian = shift.Veterinarian,
                PetId = shift.PetId,
                pet = shift.Pet,
                Description = shift.Description,
            };
        }

        public static Shift ToShift(this ShiftRequest shiftReq, Veterinarian vet, Pet pet) 
>>>>>>> 69cf1a12a291acc29e55816a3e4e48495159a2b7
        {
            return new Shift
            {
                Id = Guid.NewGuid(),
<<<<<<< HEAD
                DateShift = request.DateShift,
                Description = request.Description,
                Enrollment = veterinarian.Enrollment,
                Veterinarian = veterinarian,
                PetId = pet.Id,
                Status = Status.Pendient

            };
        
        }


=======
                DateShift = shiftReq.DateShift,
                Veterinarian = vet,
                Enrollment = shiftReq.Enrollment,
                Pet = pet,
                PetId = shiftReq.PetId,
                Description = shiftReq.Description,
            };
        }
>>>>>>> 69cf1a12a291acc29e55816a3e4e48495159a2b7
    }
}
