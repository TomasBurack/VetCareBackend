using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace VetCareBackend.Presentation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HealthController : ControllerBase
    {

        [HttpGet]
        public IActionResult Get()
        {
            return Ok("VetCare API is running!");
        }
    }
}
