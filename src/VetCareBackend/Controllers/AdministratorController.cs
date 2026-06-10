using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using VetCareBackend.Application.dtos.Requests;
using VetCareBackend.Application.Interfaces;
using VetCareBackend.Presentation.Authorization;

namespace VetCareBackend.Presentation.Controllers
{
    [Authorize(policy: Policies.soloClient)]
    [Route("api/[controller]")]
    [ApiController]

    public class AdministratorController : ControllerBase
    {
        private readonly IAdministratorService _service;
        public AdministratorController(IAdministratorService service)
        {
            _service = service;
        }

        [HttpGet("/myuser")]
        public IActionResult Get()
        {
            string? sub = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var admin = _service.Get(sub);
            return Ok(admin);
        }

        [HttpPost("/create")]
        public IActionResult Create([FromBody] SignUpRequest request)
        {
            var admin = _service.Create(request);
            return Ok(request);
        }

        [HttpDelete("/delete")]

        public IActionResult Delete()
        {
            string? sub = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            _service.Delete(sub);
            return NoContent();
        }

        [HttpPut("/update")]
        public IActionResult Update([FromBody] UserRequest request)
        {
            string? sub = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            _service.Update(sub, request);
            return NoContent();

        }
    }
}
