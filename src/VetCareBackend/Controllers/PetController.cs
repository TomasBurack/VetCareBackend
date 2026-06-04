using Microsoft.AspNetCore.Mvc;
using VetCareBackend.Application.dtos.Requests;
using VetCareBackend.Application.dtos.Responses;
using VetCareBackend.Application.Interfaces;

namespace VetCareBackend.Presentation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PetController : Controller
    {
        private readonly IPetService _petService;

        public PetController(IPetService petService)
        {
            _petService = petService;
        }

        [HttpGet]
        public ActionResult<List<PetResponse>> GetAll()
        {
            var pets = _petService.GetAll();

            if (!pets.Any())
                return NotFound("No ha mascotas registradas.");

            return Ok(pets);
        }

        [HttpGet("{id}")]
        public ActionResult<PetResponse> GetById([FromRoute] Guid id)
        {
            return Ok(_petService.GetById(id));
        }

        [HttpPost]
        public ActionResult<PetResponse> Create([FromBody] PetRequest pet)
        {
            var createdPet = _petService.Create(pet);
            return CreatedAtAction(nameof(GetById), new { id = createdPet.IdPet }, createdPet);
        }

        [HttpDelete("{id}")]
        public ActionResult Delete([FromRoute] Guid id)
        {
            _petService.Delete(id);
            return NoContent();
        }

        [HttpPut("{id}")]
        public ActionResult Update([FromBody] PetRequest pet, [FromRoute] Guid id)
        {
            _petService.Update(pet, id);
            return NoContent();
        }
    }
}
