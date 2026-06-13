using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using VetCareBackend.Application.dtos.Requests;
using VetCareBackend.Application.Interfaces;
using VetCareBackend.Domain.Entities;
using VetCareBackend.Presentation.Authorization;
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

        ///endopoints admin role ///

        [Authorize(policy: Policies.SoloAdministrator)]
        [HttpPost]
        public IActionResult Create([FromBody] VeterinarianRequest request)
        {
            var veterinarian = _veterinarianService.Create(request);
            return Ok(veterinarian);
        }
        [Authorize(policy: Policies.SoloAdministrator)]
        [HttpGet("{id}")]
        public IActionResult GetById([FromRoute] string id)
        {

            var veterinarian = _veterinarianService.GetById(id);

            return Ok(veterinarian);
        }
        [Authorize(policy: Policies.SoloAdministrator)]
        [HttpPut("{id}")]
        public IActionResult UpdateByAdmin([FromRoute] string id, [FromBody] VeterinarianRequest request)
        {
            _veterinarianService.Update(id, request);
            return NoContent();
        }

        [Authorize(policy: Policies.SoloAdministrator)]
        [HttpDelete("{id}")]
        public IActionResult Delete([FromRoute] string id)
        {
            _veterinarianService.Delete(id);
            return NoContent();
        }

        /// endpoints for admin y vet ///

        [Authorize(policy: Policies.VetAdm)]
        [HttpGet("veterinarian")]
        public IActionResult GetAll()
        {
            var veterinarians = _veterinarianService.GetAll();
            return Ok(veterinarians);
        }

        /// endpoint for vet ///

        [Authorize(policy: Policies.SoloVeterinarian)]
        [HttpGet("myuser")]
        public IActionResult Get()
        {
            string? id = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var veterinarian = _veterinarianService.GetById(id);
            return Ok(veterinarian);
        }

        [Authorize(policy: Policies.SoloVeterinarian)]
        [HttpPut("myuser")]
        public IActionResult Update([FromBody] VeterinarianRequest request)
        {
            {
                string? id = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                _veterinarianService.Update(id, request);
                return NoContent();
            }
        }

    }
}
