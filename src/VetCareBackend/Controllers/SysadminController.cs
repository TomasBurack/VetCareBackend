using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using VetCareBackend.Application.Interfaces;

namespace VetCareBackend.Presentation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SysadminController : ControllerBase
    {
        private readonly ISysadminService _service;
        public SysadminController(ISysadminService service)
        {
            _service = service;
        }
    }
}
