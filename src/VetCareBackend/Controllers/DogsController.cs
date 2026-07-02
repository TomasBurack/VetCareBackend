using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VetCareBackend.Application.Interfaces;
using VetCareBackend.Presentation.Authorization;

namespace VetCareBackend.Presentation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DogsController : ControllerBase
    {
        private readonly IDogApiService _dogService;

        public DogsController(IDogApiService dogService)
        {
            _dogService = dogService;
        }
        /// <summary>
        /// This endpoint retrieves a list of all dog breeds from the external Dog API.
        /// </summary>
        [Authorize(policy: Policies.SoloClient)]
        [HttpGet("/api/dog-breeds")]
        public async Task<IReadOnlyList<string>> GetBreeds()
        {
            var breeds = await _dogService.GetAllBreedsAsync();
            return breeds;
        }
    }
}
