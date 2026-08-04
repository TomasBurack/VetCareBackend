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
        [Authorize(policy: Policies.SoloClient)]
        [HttpGet("/api/client/pet/")]
        public async Task<ActionResult<List<PetResponse>>> GetAll()
        {
            string? sub = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var pets = await _petService.GetAll(sub);

            if (!pets.Any())
                return NotFound("No ha mascotas registradas.");

            return Ok(pets);
        }
        /// <summary>
        /// This endpoint retrieves a specific pet by its unique identifier (id).
        /// </summary>
        [Authorize(policy: Policies.SoloClient)]
        [HttpGet("/api/client/pet/{id}")]
        public async Task<ActionResult<PetResponse>> GetById([FromRoute] Guid id)
        {
            string? sub = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            return Ok(await _petService.GetById(id,sub));
        }
        /// <summary>
        /// This endpoint allows the creation of a new pet associated with the authenticated user.
        /// </summary>
        [Authorize(policy: Policies.SoloClient)]
        [HttpPost("/api/client/pet/create")]
        public async Task<ActionResult<PetResponse>> Create([FromBody] PetRequest pet)
        {
            string? sub = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var createdPet = await _petService.Create(pet, sub);
            return CreatedAtAction(nameof(GetById), new { id = createdPet.IdPet }, createdPet);
        }
        /// <summary>
        /// This endpoint allows the deletion of a pet by its unique identifier (id).
        /// </summary>
        [Authorize(policy: Policies.SoloClient)]
        [HttpDelete("/api/client/pet/delete/{id}")]
        public async Task<ActionResult> Delete([FromRoute] Guid id)
        {
            string? sub = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            await _petService.Delete(id, sub);
            return NoContent();
        }
        /// <summary>
        /// This endpoint allows the update of an existing pet's information by its unique identifier (id).
        /// </summary>
        [Authorize(policy: Policies.SoloClient)]
        [HttpPut("/api/client/pet/update/{id}")]
        public async Task<ActionResult> Update([FromBody] PetRequest pet, [FromRoute] Guid id)
        {
            string? sub = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            await _petService.Update(pet, id, sub);
            return NoContent();
        }
        /// <summary>
        /// This endpoint retrieves all pets belonging to every client in the system, including owner information.
        /// It requires the user to be authenticated and authorized as Admins(administrator or sysadmin).
        /// </summary>
        [Authorize(policy: Policies.Admins)]
        [HttpGet("/api/admins/pet/all")]
        public async Task<ActionResult<List<PetAdminResponse>>> GetAllAdmin()
        {
            var pets = await _petService.GetAllAdmin();
            return Ok(pets);
        }
        /// <summary>
        /// This endpoint allows an administrator or sysadmin to update any client's pet by its unique identifier (id).
        /// It requires the user to be authenticated and authorized as Admins(administrator or sysadmin).
        /// </summary>
        [Authorize(policy: Policies.Admins)]
        [HttpPut("/api/admins/pet/update/{id}")]
        public async Task<ActionResult> UpdateAdmin([FromBody] PetRequest pet, [FromRoute] Guid id)
        {
            await _petService.UpdatePetAdmin(pet, id);
            return NoContent();
        }
        /// <summary>
        /// This endpoint allows an administrator or sysadmin to delete any client's pet by its unique identifier (id).
        /// It requires the user to be authenticated and authorized as Admins(administrator or sysadmin).
        /// </summary>
        [Authorize(policy: Policies.Admins)]
        [HttpDelete("/api/admins/pet/delete/{id}")]
        public async Task<ActionResult> DeleteAdmin([FromRoute] Guid id)
        {
            await _petService.DeletePetAdmin(id);
            return NoContent();
        }
    }
}
