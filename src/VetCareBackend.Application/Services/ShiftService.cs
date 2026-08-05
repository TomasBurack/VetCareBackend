using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using VetCareBackend.Application.dtos.Requests;
using VetCareBackend.Application.dtos.Responses;
using VetCareBackend.Application.Exceptions;
using VetCareBackend.Application.Infrastructure;
using VetCareBackend.Application.Interfaces;
using VetCareBackend.Application.Mapper;
using VetCareBackend.Application.Validations;
using VetCareBackend.Domain.Entities;
using VetCareBackend.Domain.Enums;
using ValidationException = VetCareBackend.Application.Exceptions.ValidationException;

namespace VetCareBackend.Application.Services
{
    public class ShiftService : IShiftService
    {
        private readonly IShiftRepository _shiftRepository;
        private readonly IPetRepository _petRepository;
        private readonly IVeterinarianRepository _veterinarianRepository;

        public ShiftService(IShiftRepository shiftRepository, IPetRepository petRepository, IVeterinarianRepository veterinarianRepository)
        {
            _shiftRepository = shiftRepository;
            _petRepository = petRepository;
            _veterinarianRepository = veterinarianRepository;
        }
        public async Task<List<ShiftResponse>> GetAllAdmin()
        {
            var shifts = await _shiftRepository.GetAll();
            var pets = await _petRepository.GetAll();
            var petIds = pets
                .Select(p => p.Id)
                .ToList();
            var vet = await _veterinarianRepository.GetAll();
            var vetEnr = vet
                .Select(v => v.Enrollment)
                .ToList();
            return shifts.Where(s => petIds.Contains(s.PetId) && vetEnr.Contains(s.Enrollment)).Select(s => s.ToShiftResponse()).ToList();
        }
        public async Task<List<ShiftResponse>> GetAllVeterinarian(string sub)
        {
            bool parse = Guid.TryParse(sub, out Guid vetId);
            if (!parse)
                throw new ValidationException("El ID enviado no es válido");

            var vet = await _veterinarianRepository.Get(vetId);
            if (vet == null)
                throw new NotFoundException("No se encontró el veterinario.");

            var pets = await _petRepository.GetAll();
            var petIds = pets
                .Select(p => p.Id)
                .ToList();

            var shifts = await _shiftRepository.GetAll();
            return shifts
                .Where(s => s.Enrollment == vet.Enrollment && petIds.Contains(s.PetId))
                .Select(s => s.ToShiftResponse())
                .ToList();
        }

        public async Task<List<ShiftResponse>> GetAllClient(string sub)
        {
            bool parse = Guid.TryParse(sub, out Guid clientId);
            if (!parse)
                throw new ValidationException("El ID enviado no es válido");

            var pets = await _petRepository.GetAll();
            var petIds = pets
                .Where(p => p.IdClient == clientId)
                .Select(p => p.Id)
                .ToList();
            var vet = await _veterinarianRepository.GetAll();
            var vetEnr = vet
                .Select(v => v.Enrollment)
                .ToList();

            var shifts = await _shiftRepository.GetAll();
            return shifts
                .Where(s => petIds.Contains(s.PetId) && vetEnr.Contains(s.Enrollment))
                .Select(s => s.ToShiftResponse())
                .ToList();
        }


        public async Task<ShiftResponse> Create(ShiftRequest shiftReq, string sub)
        {
            ShiftRequestValidations validation = new ShiftRequestValidations();

            if (!validation.Validate(shiftReq).IsValid)
            {
                throw new ValidationException(validation.Validate(shiftReq).ToString("-"));
            }

            bool Parse = Guid.TryParse(sub, out Guid Id);
            if (Parse == false)
            {
                throw new ValidationException("El ID enviado no es válido");
            }

            var vets = await _veterinarianRepository.GetAll();
            var vet = vets.FirstOrDefault(v => v.Enrollment == shiftReq.Enrollment);

            if (vet == null)
                throw new NotFoundException($"No se encontró ningún veterinario con la matrícula '{shiftReq.Enrollment}'.");

            var pet = await _petRepository.Get(shiftReq.PetId);

            if (pet == null)
                throw new NotFoundException($"No se encontró ninguna mascota con el id: '{shiftReq.PetId}'");

            if(Id != pet.IdClient)
            {
                throw new ValidationException("La mascota no pertenece al cliente autenticado.");
            }

        var allShifts = await _shiftRepository.GetAll();

            bool shiftTaken = allShifts.Any(s =>
                s.Enrollment == shiftReq.Enrollment
                && s.DateShift == shiftReq.DateShift
                && s.Status != Status.Canceled);

            if (shiftTaken)
                throw new ValidationException($"El veterinario ya tiene un turno el '{shiftReq.DateShift}'.");

            var window = TimeSpan.FromMinutes(30);
            bool shiftTooClose = allShifts.Any(s =>
                s.Enrollment == shiftReq.Enrollment
                && s.DateShift != shiftReq.DateShift
                && (s.DateShift - shiftReq.DateShift) > -window
                && (s.DateShift - shiftReq.DateShift) < window);

            if (shiftTooClose)
                throw new ValidationException(
                    $"El veterinario ya tiene un turno dentro de los 30 minutos de '{shiftReq.DateShift}'. " +
                    $"Por favor elegí un horario con al menos 30 minutos de diferencia.");

            bool petShiftTaken = allShifts.Any(s =>
                s.PetId == shiftReq.PetId
                && s.DateShift == shiftReq.DateShift
                && s.Status != Status.Canceled);

            if (petShiftTaken)
                throw new ValidationException($"La mascota ya tiene un turno el '{shiftReq.DateShift}'.");

            var newShift = shiftReq.ToShift(vet, pet);
            await _shiftRepository.Add(newShift);
            return newShift.ToShiftResponse();
        }

        private static readonly TimeZoneInfo ArgentinaTimeZone =
            TimeZoneInfo.FindSystemTimeZoneById("America/Argentina/Buenos_Aires");

        public async Task<List<DateTime>> GetBusyTimes(string enrollment, DateTime date)
        {
            var window = TimeSpan.FromMinutes(30);
            var dayStart = date.Date;
            var dayEnd = dayStart.AddDays(1);

            var allShifts = await _shiftRepository.GetAll();
            var vetShifts = allShifts
                .Where(s => s.Enrollment == enrollment && s.Status != Status.Canceled)
                .ToList();

            var busyTimes = new List<DateTime>();
            for (var slot = dayStart; slot < dayEnd; slot = slot.AddMinutes(30))
            {
                var slotOffset = new DateTimeOffset(
                    DateTime.SpecifyKind(slot, DateTimeKind.Unspecified),
                    ArgentinaTimeZone.GetUtcOffset(slot));

                bool isBusy = vetShifts.Any(s =>
                    (s.DateShift - slotOffset) > -window && (s.DateShift - slotOffset) < window);

                if (isBusy)
                    busyTimes.Add(slot);
            }

            return busyTimes;
        }


        public async Task CancelStatusClient(Guid id, string sub)
        {
            bool parse = Guid.TryParse(sub, out Guid clientId);
            if (!parse)
                throw new ValidationException("El ID enviado no es válido");

            var shift = await _shiftRepository.Get(id);
            if (shift == null)
            {
                throw new NotFoundException($"No se encontró ningún turno con el id '{id}'.");
            }

            var pet = await _petRepository.Get(shift.PetId);
            if (pet == null)
                throw new NotFoundException($"No se encontró ninguna mascota con el id: '{shift.PetId}'");

            if (pet.IdClient != clientId)
            {
                throw new ValidationException("El turno no pertenece al cliente autenticado.");
            }

            if (shift.Status != Status.Pendant)
            {
                throw new ValidationException("Solo se pueden actualizar los turnos con estado 'Pendant'.");
            }

            shift.Status = Status.Canceled;
            await _shiftRepository.Update(shift);
        }

        public async Task UpdateStatusVeterinarian(Guid id, ShiftStatusRequest request, string sub)
        {
            bool parse = Guid.TryParse(sub, out Guid vetId);
            if (!parse)
                throw new ValidationException("El ID enviado no es válido");

            var vet = await _veterinarianRepository.Get(vetId);
            if (vet == null)
                throw new NotFoundException("No se encontró el veterinario.");

            var shift = await _shiftRepository.Get(id);
            if (shift == null)
            {
                throw new NotFoundException($"No se encontró ningún turno con el id '{id}'.");
            }

            if (shift.Enrollment != vet.Enrollment)
            {
                throw new ValidationException("El turno no pertenece al veterinario autenticado.");
            }

            ShiftStatusRequestValidation validations = new ShiftStatusRequestValidation();

            if(!validations.Validate(request).IsValid)
            {
                throw new ValidationException(validations.Validate(request).ToString("-"));
            }

            if (shift.Status != Status.Pendant)
            {
                throw new ValidationException("Solo se pueden actualizar los turnos con estado 'Pendant'.");
            }

            if (request.Status == Status.Served && shift.DateShift > DateTimeOffset.Now)
            {
                throw new ValidationException("No se puede marcar como 'Served' un turno cuya fecha aún no llegó.");
            }

            shift.Status = request.Status;
            await _shiftRepository.Update(shift);
        }

        public async Task UpdateObservationsVeterinarian(Guid id, ShiftObservationsRequest request, string sub)
        {
            bool parse = Guid.TryParse(sub, out Guid vetId);
            if (!parse)
                throw new ValidationException("El ID enviado no es válido");

            var vet = await _veterinarianRepository.Get(vetId);
            if (vet == null)
                throw new NotFoundException("No se encontró el veterinario.");

            var shift = await _shiftRepository.Get(id);
            if (shift == null)
            {
                throw new NotFoundException($"No se encontró ningún turno con el id '{id}'.");
            }

            if (shift.Enrollment != vet.Enrollment)
            {
                throw new ValidationException("El turno no pertenece al veterinario autenticado.");
            }

            ShiftObservationsRequestValidation validations = new ShiftObservationsRequestValidation();

            if (!validations.Validate(request).IsValid)
            {
                throw new ValidationException(validations.Validate(request).ToString("-"));
            }

            if (shift.Status != Status.Served)
            {
                throw new ValidationException("Solo se pueden agregar observaciones a los turnos con estado 'Served'.");
            }

            shift.Observations = request.Observations;
            await _shiftRepository.Update(shift);
        }

        public async Task UpdateStatusAdmin(Guid id, ShiftStatusRequest request)
        {
            var shift = await _shiftRepository.Get(id);
            if (shift == null)
            {
                throw new NotFoundException($"No se encontró ningún turno con el id '{id}'.");
            }

            ShiftStatusRequestValidation validations = new ShiftStatusRequestValidation();

            if (!validations.Validate(request).IsValid)
            {
                throw new ValidationException(validations.Validate(request).ToString("-"));
            }

            if (shift.Status != Status.Pendant)
            {
                throw new ValidationException("Solo se pueden actualizar los turnos con estado 'Pendant'.");
            }

            if (request.Status == Status.Served && shift.DateShift > DateTimeOffset.Now)
            {
                throw new ValidationException("No se puede marcar como 'Served' un turno cuya fecha aún no llegó.");
            }

            shift.Status = request.Status;
            await _shiftRepository.Update(shift);
        }

        public async Task Delete(Guid id)
        {
            var shift = await _shiftRepository.Get(id);
            if (shift == null)
            {
                throw new NotFoundException($"No se encontró ningún turno con el id '{id}'.");
            }

            if (shift.Status != Status.Pendant)
            {
                throw new ValidationException("Solo se pueden eliminar los turnos con estado 'Pendant'.");
            }

            await _shiftRepository.Delete(id);
        }
    }
}
