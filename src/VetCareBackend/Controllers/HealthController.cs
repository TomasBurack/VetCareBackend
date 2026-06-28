using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace VetCareBackend.Presentation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HealthController : ControllerBase
    {
        /// <summary>
        /// This endpoint is used to check the health of the VetCare API. It returns a simple message indicating that the API is running.
        /// </summary>
        [HttpGet]
        public IActionResult Get()
        {
            return Ok("VetCare API is running!");
        }
    }
}
