using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using VetCareBackend.Application.dtos.Requests;
using VetCareBackend.Application.dtos.Responses;
using VetCareBackend.Application.Interfaces;
using VetCareBackend.Domain.Entities;
using VetCareBackend.Presentation.Authorization;

namespace VetCareBackend.Presentation.Controllers
{
    [Authorize(policy: Policies.SoloClient)]
    [Route("api/[controller]")]
    [ApiController]
    public class PetController : Controller
    {
        private readonly IPetService _petService;
        private readonly IHttpContextAccessor _contextAccessor;

        public PetController(IPetService petService, IHttpContextAccessor contextAccessor)
        {
            _petService = petService;
            _contextAccessor = contextAccessor;
        }

        [HttpGet]
        public async Task<ActionResult<List<PetResponse>>> GetAll()
        {
            var pets = await _petService.GetAll();

            if (!pets.Any())
                return NotFound("No ha mascotas registradas.");

            return Ok(pets);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<PetResponse>> GetById([FromRoute] Guid id)
        {
            return Ok(await _petService.GetById(id));
        }

        [HttpPost]
        public async Task<ActionResult<PetResponse>> Create([FromBody] PetRequest pet)
        {
            string? sub = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var createdPet = await _petService.Create(pet, sub);
            return CreatedAtAction(nameof(GetById), new { id = createdPet.IdPet }, createdPet);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete([FromRoute] Guid id)
        {
            await _petService.Delete(id);
            return NoContent();
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> Update([FromBody] PetRequest pet, [FromRoute] Guid id)
        {
            await _petService.Update(pet, id);
            return NoContent();
        }
    }
}
