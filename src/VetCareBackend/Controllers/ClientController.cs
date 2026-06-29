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
        /// <summary>
        /// This endpoint allows a client to retrieve their own user information.
        /// </summary>
        [Authorize(policy: Policies.SoloClient)]
        [HttpGet("/myuser")]
        public async Task<IActionResult> Get()
        {
            string? sub = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var client = await _clientService.Get(sub);
            return Ok(client);
        }
        /// <summary>
        /// This endpoint allows a client to delete their own user account.
        /// </summary>
        [Authorize(policy: Policies.SoloClient)]
        [HttpDelete("/myuser/delete")]
        public async Task<IActionResult> Delete()
        {
            string? sub = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            await _clientService.Delete(sub);
            return NoContent();
        }
        /// <summary>
        /// This endpoint allows a client to update their own user information.
        /// </summary>
        [Authorize(policy: Policies.SoloClient)]
        [HttpPut("/myuser/update")]
        public async Task<IActionResult> Update([FromBody] UserRequest request)
        {
            string? sub = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            await _clientService.Update(sub, request);
            return NoContent();
        }
        /// <summary>
        /// This endpoint allows an administrator to retrieve a specific client's information by their unique identifier (Id).
        /// </summary>
        [Authorize(policy: Policies.Admins)]
        [HttpGet("/client/{Id}")]
        public async Task<IActionResult> Get([FromRoute] string Id)
        {
            var client = await _clientService.Get(Id);
            return Ok(client);
        }
        /// <summary>
        /// This endpoint allows an administrator to create a new client user account. 
        /// It accepts a SignUpRequest object in the request body and returns the created client information upon successful registration.
        /// </summary>
        [Authorize(policy: Policies.Admins)]
        [HttpPost("/client/create")]
        public async Task<IActionResult> Create([FromBody] SignUpRequest request)
        {
            var client = await _clientService.Create(request);
            return Ok(client);
        }
        /// <summary>
        /// This endpoint allows an administrator to delete a specific client user account by their unique identifier (Id).
        /// </summary>
        [Authorize(policy: Policies.Admins)]
        [HttpDelete("/client/delete/{Id}")]
        public async Task<IActionResult> Delete([FromRoute] string Id)
        {
            await _clientService.Delete(Id);
            return NoContent();
        }
        /// <summary>
        /// This endpoint allows an administrator to update a specific client's information by their unique identifier (Id).
        /// </summary>
        [Authorize(policy: Policies.Admins)]
        [HttpPut("/client/update/{Id}")]
        public async Task<IActionResult> Update([FromBody] UserRequest request, [FromRoute] string Id)
        {
            await _clientService.Update(Id, request);
            return NoContent();
        }
        /// <summary>
        /// This endpoint allows an administrator to retrieve a list of all clients in the system.
        /// </summary>
        [Authorize(policy: Policies.Admins)]
        [HttpGet("/client/all")]
        public async Task<IActionResult> GetAll()
        {
            var client = await _clientService.GetAll();
            return Ok(client);
        }
    }
}
