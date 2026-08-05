using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using VetCareBackend.Application.dtos.Requests;
using VetCareBackend.Application.Interfaces;
using VetCareBackend.Domain.Enums;
using VetCareBackend.Presentation.Authorization;

namespace VetCareBackend.Presentation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ShiftController : ControllerBase
    {
        private readonly IShiftService _shiftService;
        public ShiftController(IShiftService shiftService)
        {
            _shiftService = shiftService;
        }
        /// <summary>
        /// This endpoint allows the creation of a new shift. 
        /// It requires the user to have the SoloClient policy authorization. 
        /// The request body should contain the necessary details for creating the shift, such as date, description, pet ID, and enrollment. 
        /// Upon successful creation, it returns the created shift details.
        /// </summary>
        [Authorize(policy: Policies.SoloClient)]
        [HttpPost("/api/shift/create")]
        public async Task<IActionResult> Create([FromBody] ShiftRequest request)
        {
            string? sub = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var shift = await _shiftService.Create(request, sub);
            return Ok(shift);
        }

        /// <summary>
        /// This endpoint retrieves the times of the given day that are already taken (or within
        /// 30 minutes of another shift) for the given veterinarian, so the client can disable
        /// those time slots when picking a time for a new shift.
        /// It requires the user to have the SoloClient policy authorization.
        /// </summary>
        [Authorize(policy: Policies.SoloClient)]
        [HttpGet("/api/shift/busy-times")]
        public async Task<IActionResult> GetBusyTimes([FromQuery] string enrollment, [FromQuery] DateTime date)
        {
            var busyTimes = await _shiftService.GetBusyTimes(enrollment, date);
            return Ok(busyTimes);
        }

        /// <summary>
        /// This endpoint retrieves all shifts for administratorS, status and enrollment.
        /// It requires the user to be authenticated and authorized as Admins(administrator or sysadmin).
        /// </summary>
        [Authorize(policy: Policies.Admins)]
        [HttpGet("/api/shift/admin")]
        public async Task<IActionResult> GetAllAdmin()
        {
            var shifts = await _shiftService.GetAllAdmin();
            return Ok(shifts);
        }

        /// <summary>
        /// This endpoint retrieves all shifts belonging to the authenticated client's pets.
        /// It requires the user to have the SoloClient policy authorization.
        /// </summary>

        [Authorize(policy: Policies.SoloClient)]
        [HttpGet("/api/shift/client")]
        public async Task<IActionResult> GetAllClient()
        { 
            string? sub = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var shifts = await _shiftService.GetAllClient(sub!);
            return Ok(shifts);
        }
        /// <summary>
        /// This endpoint retrieves all shifts assigned to the authenticated veterinarian.
        /// It requires the user to have the SoloVeterinarian policy authorization
        /// </summary>

        [Authorize(policy: Policies.SoloVeterinarian)]
        [HttpGet("/api/shift/veterinarian")]
        public async Task<IActionResult> GetAllVeterinarian()
        {
            string? sub = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var shifts = await _shiftService.GetAllVeterinarian(sub!);
            return Ok(shifts);
        }

        /// <summary>
        /// This endpoint allows the update of a shift's status by its unique identifier (id) for a client.
        /// It requires the user to have the SoloClient policy authorization.
        /// </summary>
        [Authorize(policy: Policies.SoloClient)]
        [HttpPut("/api/shift/status/client/{id}")]
        public async Task<IActionResult> UpdateStatusClient(Guid id)
        {
            string? sub = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            await _shiftService.CancelStatusClient(id, sub!);
            return NoContent();
        }

        /// <summary>
        /// This endpoint allows the update of a shift's status by its unique identifier (id) for a veterinarian.
        /// It requires the user to have the SoloVeterinarian policy authorization
        /// </summary>
        [Authorize(policy: Policies.SoloVeterinarian)]
        [HttpPut("/api/shift/status/veterinarian/{id}")]
        public async Task<IActionResult> UpdateStatusVeterinarian(Guid id, [FromBody] ShiftStatusRequest request)
        {
            string? sub = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            await _shiftService.UpdateStatusVeterinarian(id, request, sub!);
            return NoContent();
        }
        /// <summary>
        /// This endpoint allows a veterinarian to add or update clinical observations on a shift
        /// by its unique identifier (id), regardless of the shift's current status.
        /// It requires the user to have the SoloVeterinarian policy authorization.
        /// </summary>
        [Authorize(policy: Policies.SoloVeterinarian)]
        [HttpPut("/api/shift/observations/{id}")]
        public async Task<IActionResult> UpdateObservationsVeterinarian(Guid id, [FromBody] ShiftObservationsRequest request)
        {
            string? sub = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            await _shiftService.UpdateObservationsVeterinarian(id, request, sub!);
            return NoContent();
        }

        /// <summary>
        /// This endpoint allows the update of a shift's status by its unique identifier (id) for an admin.
        /// It requires the user to be authenticated and authorized as Admins(administrator or sysadmin).
        /// </summary>
        [Authorize(policy: Policies.Admins)]
        [HttpPut("/api/admins/shift/status/{id}")]
        public async Task<IActionResult> UpdateStatusAdmin(Guid id, [FromBody] ShiftStatusRequest request)
        {
            await _shiftService.UpdateStatusAdmin(id, request);
            return NoContent();
        }

        /// <summary>
        /// This endpoint allows the deletion of a shift by its unique identifier (id).
        /// It requires the user to be authenticated and authorized as Admins(administrator or sysadmin).
        /// </summary>
        [Authorize(policy: Policies.Admins)]
        [HttpDelete("/api/admins/shift/delete/{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            await _shiftService.Delete(id);
            return NoContent();
        }
    }
}
