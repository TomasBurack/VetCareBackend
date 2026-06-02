using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using VetCareBackend.Application.Interfaces;
using VetCareBackend.Application.Services;
using VetCareBackend.Presentation.Authorization;
using System.Security.Claims;


namespace VetCareBackend.Presentation.Controllers
{
    [Authorize(policy: Policies.soloClient)]
    [Route("api/[controller]")]
    [ApiController]
    public class ClientController : ControllerBase
    {
        private readonly IClientService _clientService;
        private readonly IHttpContextAccessor _contextAccessor;
        public ClientController(IHttpContextAccessor contextAccessor, IClientService clientService) {
            _contextAccessor = contextAccessor;
            _clientService = clientService;
        }

        [HttpGet("/myuser")]

        public IActionResult Get()
        {
            string? sub = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var client = _clientService.Get(sub);
            return Ok(client);
        }

        [HttpDelete("/myuser/delete")]
        public IActionResult Delete()
        {
            string? sub = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            _clientService.Delete(sub);
            return NoContent();

        }

        [HttpPut("/myuser/update")]
        public IActionResult Update()
        {
            string? sub = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            _clientService.Update(sub);
            return NoContent();

        }
    }
}
