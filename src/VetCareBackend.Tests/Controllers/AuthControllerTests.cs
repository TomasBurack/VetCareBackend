using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using VetCareBackend.Application.dtos.Requests;
using VetCareBackend.Application.dtos.Responses;
using VetCareBackend.Application.Interfaces;
using VetCareBackend.Presentation.Controllers;

namespace VetCareBackend.Tests.Controllers;

public class AuthControllerTests
{
    private readonly Mock<IAuthService> _authServiceMock;
    private readonly AuthController _controller;

    public AuthControllerTests()
    {
        _authServiceMock = new Mock<IAuthService>();
        _controller = new AuthController(_authServiceMock.Object);
    }

    [Fact]
    public async Task SignUp_ReturnsStatus201_WithAuthResponse()
    {
        var request = new SignUpRequest
        {
            FirstName = "Juan",
            LastName = "Perez",
            Dni = "12345678",
            Email = "juan@test.com",
            Password = "Password123!",
            PhoneNumber = "1122334455"
        };
        var expectedResponse = new AuthResponse
        {
            Token = "jwt-token",
            Role = "Client",
            UserId = Guid.NewGuid(),
            Email = "juan@test.com"
        };
        _authServiceMock.Setup(s => s.SignUp(request)).ReturnsAsync(expectedResponse);

        var result = await _controller.SignUp(request);

        var objectResult = Assert.IsType<ObjectResult>(result.Result);
        Assert.Equal(StatusCodes.Status201Created, objectResult.StatusCode);
        var body = Assert.IsType<AuthResponse>(objectResult.Value);
        Assert.Equal("jwt-token", body.Token);
        Assert.Equal("Client", body.Role);
        _authServiceMock.Verify(s => s.SignUp(request), Times.Once);
    }

    [Fact]
    public async Task SignIn_ReturnsOk_WithAuthResponse()
    {
        var request = new SignInRequest { Email = "juan@test.com", Password = "Password123!" };
        var expectedResponse = new AuthResponse
        {
            Token = "jwt-token",
            Role = "Client",
            UserId = Guid.NewGuid(),
            Email = "juan@test.com"
        };
        _authServiceMock.Setup(s => s.SignIn(request)).ReturnsAsync(expectedResponse);

        var result = await _controller.SignIn(request);

        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var body = Assert.IsType<AuthResponse>(okResult.Value);
        Assert.Equal("jwt-token", body.Token);
        _authServiceMock.Verify(s => s.SignIn(request), Times.Once);
    }
}
