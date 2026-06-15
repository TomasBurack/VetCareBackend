using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using VetCareBackend.Application.dtos.Requests;
using VetCareBackend.Application.Interfaces;
using VetCareBackend.Presentation.Authorization;

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

        /*[Authorize(policy: Policies.Admins)]
        [HttpPost("/sysadmin/create")]
        public IActionResult Create([FromBody] SignUpRequest request)
        {
            var sysadmin = _service.Create(request);
            return Ok(sysadmin);
        }*/

        [Authorize(policy: Policies.SoloSysadmin)]
        [HttpGet("/sysadmin/myuser")]
        public IActionResult Get()
        {
            string? sub = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var sysadmin = _service.Get(sub);
            return Ok(sysadmin);
        }

        [Authorize(policy: Policies.SoloSysadmin)]
        [HttpDelete("/sysadmin/myuser/delete")]
        public IActionResult Delete()
        {
            string? sub = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            _service.Delete(sub);
            return NoContent();
        }

        [Authorize(policy: Policies.SoloSysadmin)]
        [HttpPut("/sysadmin/myuser/update")]
        public IActionResult Update([FromBody] UserRequest request)
        {
            string? sub = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            _service.Update(sub, request);
            return NoContent();

        }

        [Authorize(policy: Policies.SoloSysadmin)]
        [HttpGet("/sysadmin/all")]
        public IActionResult GetAll()
        {
            var sysadmin = _service.GetAll();
            return Ok(sysadmin);
        }
    }
}
