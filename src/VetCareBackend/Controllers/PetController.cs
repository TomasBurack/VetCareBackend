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
        /// <summary>
        /// This endpoint retrieves all pets associated with the authenticated user. 
        /// It returns a list of PetResponse objects if pets are found, or a NotFound response if no pets are registered for the user.
        /// </summary>
        [HttpGet]
        public async Task<ActionResult<List<PetResponse>>> GetAll()
        {
            var pets = await _petService.GetAll();

            if (!pets.Any())
                return NotFound("No ha mascotas registradas.");

            return Ok(pets);
        }
        /// <summary>
        /// This endpoint retrieves a specific pet by its unique identifier (id).
        /// </summary>
        [HttpGet("{id}")]
        public async Task<ActionResult<PetResponse>> GetById([FromRoute] Guid id)
        {
            return Ok(await _petService.GetById(id));
        }
        /// <summary>
        /// This endpoint allows the creation of a new pet associated with the authenticated user.
        /// </summary>
        [HttpPost]
        public async Task<ActionResult<PetResponse>> Create([FromBody] PetRequest pet)
        {
            string? sub = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var createdPet = await _petService.Create(pet, sub);
            return CreatedAtAction(nameof(GetById), new { id = createdPet.IdPet }, createdPet);
        }
        /// <summary>
        /// This endpoint allows the deletion of a pet by its unique identifier (id).
        /// </summary>
        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete([FromRoute] Guid id)
        {
            await _petService.Delete(id);
            return NoContent();
        }
        /// <summary>
        /// This endpoint allows the update of an existing pet's information by its unique identifier (id).
        /// </summary>
        [HttpPut("{id}")]
        public async Task<ActionResult> Update([FromBody] PetRequest pet, [FromRoute] Guid id)
        {
            await _petService.Update(pet, id);
            return NoContent();
        }
    }
}
