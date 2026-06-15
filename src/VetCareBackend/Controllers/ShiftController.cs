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
      public IActionResult Create([FromBody] ShiftRequest request)
        {
            var shift = _shiftService.Create(request);
            return Ok(shift);
        }


        [Authorize(policy: Policies.VetAdm)]
        [HttpGet]
        public IActionResult GetAll()
        {
            var shift = _shiftService.GetAll();
            return Ok(shift);
        }
        [Authorize(policy: Policies.VetAdm)]
        [HttpPatch("status/{id}")]
        public IActionResult UpdateStatus(Guid id, [FromBody] ShiftStatusRequest request)
        {
            _shiftService.UpdateStatus(id, request);
            return NoContent();
        }
        [Authorize(policy: Policies.VetAdm)]
        [HttpDelete("delete/{id}")]
        public IActionResult Delete(Guid id)
        {
            _shiftService.Delete(id);
            return NoContent();
        }


    }
}
