using Microsoft.AspNetCore.Mvc;
using Moq;
using VetCareBackend.Application.Interfaces;
using VetCareBackend.Presentation.Controllers;

namespace VetCareBackend.Tests.Controllers;

public class MailControllerTests
{
    private readonly Mock<IMailService> _mailServiceMock;
    private readonly MailController _controller;

    public MailControllerTests()
    {
        _mailServiceMock = new Mock<IMailService>();
        _controller = new MailController(_mailServiceMock.Object);
    }

    [Fact]
    public async Task SendEmail_ReturnsOk()
    {
        _mailServiceMock.Setup(s => s.SendEmail()).Returns(Task.CompletedTask);

        var result = await _controller.SendEmail();

        Assert.IsType<OkResult>(result);
        _mailServiceMock.Verify(s => s.SendEmail(), Times.Once);
    }

    [Fact]
    public async Task SendEmail_ThrowsException_WhenServiceFails()
    {
        _mailServiceMock.Setup(s => s.SendEmail()).ThrowsAsync(new Exception("SMTP error"));

        await Assert.ThrowsAsync<Exception>(() => _controller.SendEmail());
    }
}
