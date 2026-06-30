using Microsoft.AspNetCore.Mvc;
using VetCareBackend.Application.Interfaces;

namespace VetCareBackend.Presentation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CatsController : ControllerBase
    {
        private readonly ICatApiService _catService;

        public CatsController(ICatApiService catService)
        {
            _catService = catService;
        }
        /// <summary>
        /// This endpoint retrieves a list of all cat breeds from the external cat API service.
        /// </summary>
        [HttpGet("breeds")]
        public async Task<IReadOnlyList<string>> GetBreeds()
        {
            var breeds = await _catService.GetAllBreedsAsync();
            return breeds;
        }
    }
}
