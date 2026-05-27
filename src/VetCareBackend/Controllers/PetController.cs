using Microsoft.AspNetCore.Mvc;
using VetCareBackend.Application.dtos.Requests;
using VetCareBackend.Application.dtos.Responses;
using VetCareBackend.Application.Exceptions;
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
        public ActionResult<List<PetResponse>>  GetAll()
        {
            try
            {
                var pets = _petService.GetAll();

                if (!pets.Any())
                    return NotFound("No ha mascotas registradas.");

                return Ok(pets);
            }
            catch (DatabaseException ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpGet("({id")]
        public ActionResult<PetResponse> GetById([FromRoute]Guid id)
        {
            try
            {
                return Ok(_petService.GetById(id));
            }
            catch (NotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (DatabaseException ex)
            {
                return StatusCode(500, ex.Message);
            }
            catch (Exception)
            {
                return StatusCode(500, "Ocurrio un error inesperado.");
            }
        }

        [HttpPost]
        public ActionResult<PetResponse> Create([FromBody] PetRequest pet)
        {
            try
            {
                var createdPet = _petService.Create(pet);
                return CreatedAtAction(nameof(GetById), new { id = createdPet.IdPet }, createdPet);
            }
            catch (DatabaseException ex)
            {
                return StatusCode(500, ex.Message);
            }
            catch (Exception)
            {
                return StatusCode(500, "Ocurrió un error inesperado.");
            }
        }

        [HttpDelete("{id}")]
        public ActionResult Delete([FromRoute] Guid id) 
        {
            try
            {
                _petService.Delete(id);
                return NoContent();
            }
            catch (NotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (DatabaseException ex)
            {
                return StatusCode(500, ex.Message);
            }
            catch (Exception)
            {
                return StatusCode(500, "Ocurrió un error inesperado.");
            }
        }

        [HttpPut("{id}")]
        public ActionResult Update([FromBody] PetRequest pet, [FromRoute] Guid id)
        {
            try
            {
                _petService.Update(pet, id);
                return NoContent();
            }
            catch (NotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (DatabaseException ex)
            {
                return StatusCode(500, ex.Message);
            }
            catch (Exception)
            {
                return StatusCode(500, "Ocurrió un error inesperado.");
            }
        }
    }
}
