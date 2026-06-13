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

        //routes for client role
        
        [Authorize(policy: Policies.SoloClient)]
        [HttpGet("/myuser")]
        public IActionResult Get()
        {
            string? sub = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var client = _clientService.Get(sub);
            return Ok(client);
        }

        [Authorize(policy: Policies.SoloClient)]
        [HttpDelete("/myuser/delete")]
        public IActionResult Delete()
        {
            string? sub = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            _clientService.Delete(sub);
            return NoContent();
        }

        [Authorize(policy: Policies.SoloClient)]
        [HttpPut("/myuser/update")]
        public IActionResult Update([FromBody] UserRequest request)
        {
            string? sub = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            _clientService.Update(sub, request);
            return NoContent();
        }


        //routes for admins role

        [Authorize(policy: Policies.Admins)]
        [HttpGet("/client/{Id}")]
        public IActionResult Get([FromRoute] string Id)
        {
            var client = _clientService.Get(Id);
            return Ok(client);
        }

        [Authorize(policy: Policies.Admins)]
        [HttpPost("/client/create")]
        public IActionResult Create([FromBody] SignUpRequest request)
        {
            var client = _clientService.Create(request);
            return Ok(client);
        }

        [Authorize(policy: Policies.Admins)]
        [HttpDelete("/client/delete/{Id}")]
        public IActionResult Delete([FromRoute] string Id)
        {
            _clientService.Delete(Id);
            return NoContent();
        }

        [Authorize(policy: Policies.Admins)]
        [HttpPut("/client/update/{Id}")]
        public IActionResult Update([FromBody] UserRequest request,[FromRoute] string Id)
        {
            _clientService.Update(Id, request);
            return NoContent();
        }

        [Authorize(policy: Policies.Admins)]
        [HttpGet("/client/all")]
        public IActionResult GetAll()
        {
            var client = _clientService.GetAll();
            return Ok(client);
        }
    }
}
