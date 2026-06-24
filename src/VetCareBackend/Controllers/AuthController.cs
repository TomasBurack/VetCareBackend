using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using VetCareBackend.Application.dtos.Requests;
using VetCareBackend.Application.dtos.Responses;
using VetCareBackend.Application.Interfaces;
using VetCareBackend.Infrastructure.ExternalService;

namespace VetCareBackend.Presentation.Controllers
{
    [Route("api/auth")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _service;
        public AuthController(IAuthService service)
        {
            _service = service;
        }

        [HttpPost("signup")]
        [AllowAnonymous]
        public async Task<ActionResult<AuthResponse>> SignUp([FromBody] SignUpRequest request)
        {
            var response = await _service.SignUp(request);
            return StatusCode(StatusCodes.Status201Created, response);
        }

        [HttpPost("signin")]
        [AllowAnonymous]
        public async Task<ActionResult<AuthResponse>> SignIn([FromBody] SignInRequest request)
        {
            return Ok(await _service.SignIn(request));
        }

        [HttpPost("forgost-post")]
        [AllowAnonymous]
        public async Task<ActionResult> ForgotPassword([FromBody] ForgotPasswordRequest request)
        {
            await _service.ForgotPassword(request);
            return Ok("Si el mail existe, recibiras las instrucciones");
        }

        [HttpPost("reset-password")]
        [AllowAnonymous]
        public async Task<ActionResult> ResetPassword([FromBody] ResetPasswordRequest request) 
        {
            await _service.ResetPassword(request);
            return Ok("Contraseña actualizada correctamente");
        }
    }
}
