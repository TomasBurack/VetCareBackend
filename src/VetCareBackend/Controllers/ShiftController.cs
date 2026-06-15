using Microsoft.AspNetCore.Mvc;
using VetCareBackend.Application.dtos.Requests;
using VetCareBackend.Application.Interfaces;
using VetCareBackend.Domain.Enums;

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

      [HttpPost("/create")]
      public IActionResult Create([FromBody] ShiftRequest request)
        {
            var shift = _shiftService.Create(request);
            return Ok(shift);
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            var shift = _shiftService.GetAll();
            return Ok(shift);
        }

        [HttpPatch("status/{id}")]
        public IActionResult UpdateStatus(Guid id, [FromBody] ShiftStatusRequest request)
        {
            _shiftService.UpdateStatus(id, request);
            return NoContent();
        }

        [HttpDelete("delete/{id}")]
        public IActionResult Delete(Guid id)
        {
            _shiftService.Delete(id);
            return NoContent();
        }


    }
}
