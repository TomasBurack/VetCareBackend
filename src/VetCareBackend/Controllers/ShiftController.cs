using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
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

        [Authorize(policy: Policies.ClientAdm)]
        [HttpPost("/create")]
        public async Task<IActionResult> Create([FromBody] ShiftRequest request)
        {
            var shift = await _shiftService.Create(request);
            return Ok(shift);
        }

        [Authorize(policy: Policies.VetAdm)]
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var shift = await _shiftService.GetAll();
            return Ok(shift);
        }

        [Authorize(policy: Policies.VetAdm)]
        [HttpPatch("status/{id}")]
        public async Task<IActionResult> UpdateStatus(Guid id, [FromBody] ShiftStatusRequest request)
        {
            await _shiftService.UpdateStatus(id, request);
            return NoContent();
        }

        [Authorize(policy: Policies.VetAdm)]
        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            await _shiftService.Delete(id);
            return NoContent();
        }
    }
}
