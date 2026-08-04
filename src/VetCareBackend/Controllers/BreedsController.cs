using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VetCareBackend.Application.Interfaces;
using VetCareBackend.Domain.Enums;
using VetCareBackend.Presentation.Authorization;

namespace VetCareBackend.Presentation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BreedsController : ControllerBase
    {
        private readonly IBreedService _breedService;

        public BreedsController(IBreedService breedService)
        {
            _breedService = breedService;
        }
        /// <summary>
        /// This endpoint retrieves the list of breeds for the given pet type (TypePet), sourced from the corresponding external API.
        /// </summary>
        [Authorize(policy: Policies.ClientAdm)]
        [HttpGet("/api/breeds")]
        public async Task<IReadOnlyList<string>> GetBreeds([FromQuery] TypePet typePet)
        {
            return await _breedService.GetBreedsByTypeAsync(typePet);
        }
    }
}
