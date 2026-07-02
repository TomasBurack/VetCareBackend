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
        /// It requires the user to have the ClientAdm policy authorization. 
        /// The request body should contain the necessary details for creating the shift, such as date, description, pet ID, and enrollment. 
        /// Upon successful creation, it returns the created shift details.
        /// </summary>
        [Authorize(policy: Policies.SoloClient)]
        [HttpPost("/create")]
        public async Task<IActionResult> Create([FromBody] ShiftRequest request)
        {
            string? sub = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var shift = await _shiftService.Create(request, sub);
            return Ok(shift);
        }

        /// <summary>
        /// This endpoint retrieves all shifts for administratorS, status and enrollment.
        /// </summary>
        [Authorize(policy: Policies.Admins)]
        [HttpGet("admin")]
        public async Task<IActionResult> GetAllAdmin()
        {
            var shifts = await _shiftService.GetAllAdmin();
            return Ok(shifts);
        }

        /// <summary>
        /// This endpoint retrieves all shifts belonging to the authenticated client's pets.
        /// </summary>

        [Authorize(policy: Policies.SoloClient)]
        [HttpGet("client")]
        public async Task<IActionResult> GetAllClient()
        { 
            string? sub = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var shifts = await _shiftService.GetAllClient(sub!);
            return Ok(shifts);
        }
        /// <summary>
        /// This endpoint retrieves all shifts assigned to the authenticated veterinarian.
        /// </summary>

        [Authorize(policy: Policies.SoloVeterinarian)]
        [HttpGet("veterinarian")]
        public async Task<IActionResult> GetAllVeterinarian()
        {
            string? sub = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var shifts = await _shiftService.GetAllVeterinarian(sub!);
            return Ok(shifts);
        }

        /// <summary>
        /// This endpoint allows the update of a shift's status by its unique identifier (id).
        /// </summary>
        [Authorize(policy: Policies.VetAdm)]
        [HttpPut("status/{id}")]
        public async Task<IActionResult> UpdateStatus(Guid id, [FromBody] ShiftStatusRequest request)
        {
            await _shiftService.UpdateStatus(id, request);
            return NoContent();
        }
        /// <summary>
        /// This endpoint allows the deletion of a shift by its unique identifier (id).
        /// </summary>
        [Authorize(policy: Policies.Admins)]
        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            await _shiftService.Delete(id);
            return NoContent();
        }
    }
}
