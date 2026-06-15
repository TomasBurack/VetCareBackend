using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using VetCareBackend.Application.Interfaces;
using VetCareBackend.Infrastructure.ExternalService;

namespace VetCareBackend.Presentation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MailController : ControllerBase
    {
        private readonly IMailService _mailService;
        public MailController(IMailService mailService)
        {
            _mailService = mailService;
        }

        [HttpPost("sendemail")]
        public IActionResult SendEmail()
        {
            _mailService.SendEmail();
            return Ok();
        }
    }
}
