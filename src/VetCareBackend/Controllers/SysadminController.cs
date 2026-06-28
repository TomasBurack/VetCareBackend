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
        /// <summary>
        /// This endpoint retrieves the information of the currently authenticated sysadmin user. It requires the user to be authorized
        /// </summary>
        [Authorize(policy: Policies.SoloSysadmin)]
        [HttpGet("/sysadmin/myuser")]
        public async Task<IActionResult> Get()
        {
            string? sub = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var sysadmin = await _service.Get(sub);
            return Ok(sysadmin);
        }
        /// <summary>
        /// This endpoint deletes the currently authenticated sysadmin user.
        /// </summary>
        [Authorize(policy: Policies.SoloSysadmin)]
        [HttpDelete("/sysadmin/myuser/delete")]
        public async Task<IActionResult> Delete()
        {
            string? sub = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            await _service.Delete(sub);
            return NoContent();
        }
        /// <summary>
        /// This endpoint updates the information of the currently authenticated sysadmin user. 
        /// It requires the user to be authorized and provides a UserRequest object in the request body containing the updated information.
        /// </summary>
        [Authorize(policy: Policies.SoloSysadmin)]
        [HttpPut("/sysadmin/myuser/update")]
        public async Task<IActionResult> Update([FromBody] UserRequest request)
        {
            string? sub = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            await _service.Update(sub, request);
            return NoContent();
        }
        /// <summary>
        /// This endpoint retrieves a list of all sysadmin users in the system. It requires the user to be authorized and returns a list of sysadmin information.
        /// </summary>
        [Authorize(policy: Policies.SoloSysadmin)]
        [HttpGet("/sysadmin/all")]
        public async Task<IActionResult> GetAll()
        {
            var sysadmin = await _service.GetAll();
            return Ok(sysadmin);
        }
    }
}
