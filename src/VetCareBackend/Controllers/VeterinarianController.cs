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

        [Authorize(policy: Policies.Admins)]
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] VeterinarianRequest request)
        {
            var veterinarian = await _veterinarianService.Create(request);
            return Ok(veterinarian);
        }

        [Authorize(policy: Policies.Admins)]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById([FromRoute] string id)
        {
            var veterinarian = await _veterinarianService.GetById(id);
            return Ok(veterinarian);
        }

        [Authorize(policy: Policies.Admins)]
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateByAdmin([FromRoute] string id, [FromBody] VeterinarianUpdateRequest request)
        {
            await _veterinarianService.Update(id, request);
            return NoContent();
        }

        [Authorize(policy: Policies.SoloAdministrator)]
        [HttpDelete("{id}")]
        public async Task<IActionResult> AdminDelete([FromRoute] string id)
        {
            await _veterinarianService.Delete(id);
            return NoContent();
        }

        [Authorize(policy: Policies.Admins)]
        [HttpGet("veterinarian")]
        public async Task<IActionResult> GetAll()
        {
            var veterinarians = await _veterinarianService.GetAll();
            return Ok(veterinarians);
        }

        [Authorize(policy: Policies.SoloVeterinarian)]
        [HttpGet("myuser")]
        public async Task<IActionResult> Get()
        {
            string? id = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var veterinarian = await _veterinarianService.GetById(id);
            return Ok(veterinarian);
        }

        [Authorize(policy: Policies.SoloVeterinarian)]
        [HttpPut("myuser")]
        public async Task<IActionResult> Update([FromBody] VeterinarianUpdateRequest request)
        {
            string? id = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            await _veterinarianService.Update(id, request);
            return NoContent();
        }

        [Authorize(policy: Policies.SoloVeterinarian)]
        [HttpDelete("myuser")]
        public async Task<IActionResult> Delete()
        {
            string? id = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            await _veterinarianService.Delete(id);
            return NoContent();
        }
    }
}
