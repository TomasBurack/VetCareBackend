using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using VetCareBackend.Application.Interfaces;
using VetCareBackend.Presentation.Authorization;
using System.Security.Claims;
using VetCareBackend.Application.dtos.Requests;

namespace VetCareBackend.Presentation.Controllers
{
    
    [Route("api/[controller]")]
    [ApiController]
    public class ClientController : ControllerBase
    {
        private readonly IClientService _clientService;
        private readonly IHttpContextAccessor _contextAccessor;

        public ClientController(IHttpContextAccessor contextAccessor, IClientService clientService)
        {
            _contextAccessor = contextAccessor;
            _clientService = clientService;
        }

        [Authorize(policy: Policies.soloAdministrator)]
        [HttpPost("/client/create")]
        public IActionResult Create([FromBody] SignUpRequest request)
        {
            var client = _clientService.Create(request);
            return Ok(client);
        }

        [Authorize(policy: Policies.soloClient)]
        [HttpGet("/myuser")]
        public IActionResult Get()
        {
            string? sub = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var client = _clientService.Get(sub);
            return Ok(client);
        }
        [Authorize(policy: Policies.soloClient)]
        [HttpDelete("/myuser/delete")]
        public IActionResult Delete()
        {
            string? sub = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            _clientService.Delete(sub);
            return NoContent();
        }

        [Authorize(policy: Policies.soloClient)]
        [HttpPut("/myuser/update")]
        public IActionResult Update([FromBody] UserRequest request)
        {
            string? sub = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            _clientService.Update(sub, request);
            return NoContent();
        }
    }
}
