using Microsoft.AspNetCore.Mvc;
using VetCareBackend.Application.dtos.Requests;
using VetCareBackend.Application.Interfaces;
using System.Security.Claims;
namespace VetCareBackend.Presentation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VeterinarianController : ControllerBase
    {
        private readonly IVeterinarianService _veterinarianService;

        public VeterinarianController(IVeterinarianService veterinarianService)
        {
            _veterinarianService = veterinarianService;
        }
        [HttpPost("/veterinarian/create")]
        public IActionResult Create([FromBody] VeterinarianRequest request)
        {
            _veterinarianService.Create(request);
            return Ok("Veterinarian created successful");
        }

        [HttpGet("/veterinarian")]
        public IActionResult GetAll()
        {
            var veterinarians = _veterinarianService.GetAll();
            return Ok(veterinarians);
        }


        [HttpGet("/veterinarian/myUser")]
        public IActionResult GetById()
        {
            string? id = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            var veterinarian = _veterinarianService.GetById(id);

            return Ok(veterinarian);
        }

        [HttpPut("/veterinarian/{id}")]
        public IActionResult Update(string id, [FromBody] VeterinarianRequest request)
        {
            _veterinarianService.Update(id, request);
            return NoContent();
        }

        [HttpDelete("/veterinarian/{id}")]
        public IActionResult Delete(string id)
        {
            _veterinarianService.Delete(id);
            return NoContent();
        }
    }      
    
}
