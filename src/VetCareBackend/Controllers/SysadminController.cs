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

        [Authorize(policy: Policies.SoloSysadmin)]
        [HttpGet("/sysadmin/myuser")]
        public async Task<IActionResult> Get()
        {
            string? sub = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var sysadmin = await _service.Get(sub);
            return Ok(sysadmin);
        }

        [Authorize(policy: Policies.SoloSysadmin)]
        [HttpDelete("/sysadmin/myuser/delete")]
        public async Task<IActionResult> Delete()
        {
            string? sub = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            await _service.Delete(sub);
            return NoContent();
        }

        [Authorize(policy: Policies.SoloSysadmin)]
        [HttpPut("/sysadmin/myuser/update")]
        public async Task<IActionResult> Update([FromBody] UserRequest request)
        {
            string? sub = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            await _service.Update(sub, request);
            return NoContent();
        }

        [Authorize(policy: Policies.SoloSysadmin)]
        [HttpGet("/sysadmin/all")]
        public async Task<IActionResult> GetAll()
        {
            var sysadmin = await _service.GetAll();
            return Ok(sysadmin);
        }
    }
}
