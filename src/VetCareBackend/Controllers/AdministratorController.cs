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

        [Authorize(policy: Policies.SoloAdministrator)]
        [HttpGet("/admin/myuser")]
        public async Task<IActionResult> Get()
        {
            string? sub = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var admin = await _service.Get(sub);
            return Ok(admin);
        }

        [Authorize(policy: Policies.SoloAdministrator)]
        [HttpDelete("/admin/myuser/delete")]
        public async Task<IActionResult> Delete()
        {
            string? sub = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            await _service.Delete(sub);
            return NoContent();
        }

        [Authorize(policy: Policies.SoloAdministrator)]
        [HttpPut("/admin/myuser/update")]
        public async Task<IActionResult> Update([FromBody] UserRequest request)
        {
            string? sub = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            await _service.Update(sub, request);
            return NoContent();
        }

        [Authorize(policy: Policies.SoloSysadmin)]
        [HttpPost("/admin/create")]
        public async Task<IActionResult> Create([FromBody] SignUpRequest request)
        {
            var admin = await _service.Create(request);
            return Ok(admin);
        }

        [Authorize(policy: Policies.SoloSysadmin)]
        [HttpGet("/admin/{Id}")]
        public async Task<IActionResult> Get([FromRoute] string Id)
        {
            var admin = await _service.Get(Id);
            return Ok(admin);
        }

        [Authorize(policy: Policies.SoloSysadmin)]
        [HttpDelete("/admin/delete/{Id}")]
        public async Task<IActionResult> Delete([FromRoute] string Id)
        {
            await _service.Delete(Id);
            return NoContent();
        }

        [Authorize(policy: Policies.SoloSysadmin)]
        [HttpPut("/admin/update/{Id}")]
        public async Task<IActionResult> Update([FromBody] UserRequest request, [FromRoute] string Id)
        {
            await _service.Update(Id, request);
            return NoContent();
        }

        [Authorize(policy: Policies.SoloSysadmin)]
        [HttpGet("/admin/all")]
        public async Task<IActionResult> GetAll()
        {
            var admin = await _service.GetAll();
            return Ok(admin);
        }

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
