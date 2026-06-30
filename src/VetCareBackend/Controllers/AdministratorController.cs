using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using VetCareBackend.Application.dtos.Requests;
using VetCareBackend.Application.dtos.Responses;
using VetCareBackend.Application.Interfaces;
using VetCareBackend.Application.Services;
using VetCareBackend.Presentation.Authorization;

namespace VetCareBackend.Presentation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdministratorController : ControllerBase
    {
        private readonly IAdministratorService _service;
        private readonly IVeterinarianService _vetService;
        private readonly IClientService _clientService;
        private readonly ISysadminService _sysadminService;
        public AdministratorController(IAdministratorService service, IVeterinarianService vetService, IClientService clientService, ISysadminService sysadminService)
        {
            _service = service;
            _vetService = vetService;
            _clientService = clientService;
            _sysadminService = sysadminService;
        }

        /// <summary>
        /// This enpoint allows an administrator to retrieve their own user information. 
        /// It requires the user to be authenticated and authorized as a SoloAdministrator. 
        /// The endpoint extracts the user's unique identifier (sub) from the claims and uses it to fetch the corresponding administrator details from the service layer. 
        /// The response is returned in an HTTP 200 OK status with the administrator's information.
        /// </summary>
        [Authorize(policy: Policies.SoloAdministrator)]
        [HttpGet("/admin/myuser")]
        public async Task<IActionResult> Get()
        {
            string? sub = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var admin = await _service.Get(sub);
            return Ok(admin);
        }
        /// <summary>
        /// This endpoint allows an administrator to delete their own user account.
        /// </summary>
        [Authorize(policy: Policies.SoloAdministrator)]
        [HttpDelete("/admin/myuser/delete")]
        public async Task<IActionResult> Delete()
        {
            string? sub = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            await _service.Delete(sub);
            return NoContent();
        }
        /// <summary>
        /// This endpoint allows an administrator to update their own user information.
        /// </summary>
        [Authorize(policy: Policies.SoloAdministrator)]
        [HttpPut("/admin/myuser/update")]
        public async Task<IActionResult> Update([FromBody] UserRequest request)
        {
            string? sub = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            await _service.Update(sub, request);
            return NoContent();
        }
        /// <summary>
        /// This endpoint allows a system administrator to create a new administrator user.
        /// It requires the user to be authenticated and authorized as a SoloSysadmin. 
        /// The endpoint accepts a SignUpRequest object in the request body, which contains the necessary information for creating the new administrator. 
        /// The service layer handles the creation process, and upon successful creation, the endpoint returns an HTTP 200 OK status with the newly created administrator's information.
        /// </summary>
        [Authorize(policy: Policies.SoloSysadmin)]
        [HttpPost("/admin/create")]
        public async Task<IActionResult> Create([FromBody] SignUpRequest request)
        {
            var admin = await _service.Create(request);
            return Ok(admin);
        }
        /// <summary>
        /// This endpoint allows a system administrator to retrieve information about a specific administrator user by their unique identifier (Id).
        /// </summary>
        [Authorize(policy: Policies.SoloSysadmin)]
        [HttpGet("/admin/{Id}")]
        public async Task<IActionResult> Get([FromRoute] string Id)
        {
            var admin = await _service.Get(Id);
            return Ok(admin);
        }
        /// <summary>
        /// This endpoint allows a system administrator to delete a specific administrator user by their unique identifier (Id).
        /// </summary>
        [Authorize(policy: Policies.SoloSysadmin)]
        [HttpDelete("/admin/delete/{Id}")]
        public async Task<IActionResult> Delete([FromRoute] string Id)
        {
            await _service.Delete(Id);
            return NoContent();
        }
        /// <summary>
        /// This endpoint allows a system administrator to update the information of a specific administrator user by their unique identifier (Id).
        /// </summary>
        [Authorize(policy: Policies.SoloSysadmin)]
        [HttpPut("/admin/update/{Id}")]
        public async Task<IActionResult> Update([FromBody] UserRequest request, [FromRoute] string Id)
        {
            await _service.Update(Id, request);
            return NoContent();
        }
        /// <summary>
        /// This endpoint allows a system administrator to retrieve a list of all administrator users in the system.
        /// </summary>
        [Authorize(policy: Policies.SoloSysadmin)]
        [HttpGet("/admin/all")]
        public async Task<IActionResult> GetAll()
        {
            var admin = await _service.GetAll();
            return Ok(admin);
        }
        /// <summary>
        /// This endpoint allows a system administrator to retrieve a list of all users in the system, including administrators, clients, and veterinarians.
        /// </summary>
        [Authorize(policy: Policies.Admins)]
        [HttpGet("/alluser")]
        public async Task<IActionResult> GetAllUsers()
        {
            var admins = await _service.GetAll();
            var clients = await _clientService.GetAll();
            var vets = await _vetService.GetAll();
            var sysadmins = await _sysadminService.GetAll();
            return Ok(new { admins, clients, vets });
        }
    }
}
