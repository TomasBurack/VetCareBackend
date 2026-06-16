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

        //routes for administrator

        [Authorize(policy: Policies.SoloAdministrator)]
        [HttpGet("/admin/myuser")]
        public IActionResult Get()
        {
            string? sub = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var admin = _service.Get(sub);
            return Ok(admin);
        }

        [Authorize(policy: Policies.SoloAdministrator)]
        [HttpDelete("/admin/myuser/delete")]
        public IActionResult Delete()
        {
            string? sub = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            _service.Delete(sub);
            return NoContent();
        }

        [Authorize(policy: Policies.SoloAdministrator)]
        [HttpPut("/admin/myuser/update")]
        public IActionResult Update([FromBody] UserRequest request)
        {
            string? sub = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            _service.Update(sub, request);
            return NoContent();

        }

        [Authorize(policy: Policies.SoloAdministrator)]
        [HttpPost("/addrole")]
        public IActionResult AddRoleToUser([FromBody] AddRoleRequest request)
        {
            _service.AddRoleToUser(request.Email, request.RoleName, request.enrollment);
            return NoContent();
        }

        //routes for sysadmin role

        [Authorize(policy: Policies.SoloSysadmin)]
        [HttpPost("/admin/create")]
        public IActionResult Create([FromBody] SignUpRequest request)
        {
            var admin = _service.Create(request);
            return Ok(admin);
        }

        [Authorize(policy: Policies.SoloSysadmin)]
        [HttpGet("/admin/{Id}")]
        public IActionResult Get([FromRoute] string Id)
        {
            var admin = _service.Get(Id);
            return Ok(admin);
        }


        [Authorize(policy: Policies.SoloSysadmin)]
        [HttpDelete("/admin/delete/{Id}")]
        public IActionResult Delete([FromRoute] string Id)
        {
            _service.Delete(Id);
            return NoContent();
        }

        [Authorize(policy: Policies.SoloSysadmin)]
        [HttpPut("/admin/update/{Id}")]
        public IActionResult Update([FromBody] UserRequest request, [FromRoute] string Id)
        {
            _service.Update(Id, request);
            return NoContent();
        }

        [Authorize(policy: Policies.SoloSysadmin)]
        [HttpGet("/admin/all")]
        public IActionResult GetAll()
        {
            var admin = _service.GetAll();
            return Ok(admin);
        }

        //routes for both
        [Authorize(policy: Policies.Admins)]
        [HttpGet("/alluser")]

        public IActionResult GetAllUsers()
        {
            var admins = _service.GetAll();
            var clients = _clientService.GetAll();
            var vets = _vetService.GetAll();
            var sysadmins = _sysadminService.GetAll();
            return Ok(new { admins, clients, vets});
        }

        
    }
}
