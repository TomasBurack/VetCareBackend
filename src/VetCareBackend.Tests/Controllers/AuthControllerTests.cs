using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
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

        var configValues = new Dictionary<string, string?>
        {
            { "Jwt:ExpirationMinutes", "60" }
        };
        IConfiguration configuration = new ConfigurationBuilder()
            .AddInMemoryCollection(configValues)
            .Build();

        _controller = new AuthController(_authServiceMock.Object, configuration)
        {
            ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext()
            }
        };
    }

    [Fact]
    public async Task SignUp_ReturnsStatus201_WithAuthResponse_TokenSetAsCookie()
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
        Assert.Equal(string.Empty, body.Token);
        Assert.Equal("Client", body.Role);
        Assert.Contains("access_token", _controller.Response.Headers.SetCookie.ToString());
        _authServiceMock.Verify(s => s.SignUp(request), Times.Once);
    }

    [Fact]
    public async Task SignIn_ReturnsOk_WithAuthResponse_TokenSetAsCookie()
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
        Assert.Equal(string.Empty, body.Token);
        Assert.Contains("access_token", _controller.Response.Headers.SetCookie.ToString());
        _authServiceMock.Verify(s => s.SignIn(request), Times.Once);
    }

    [Fact]
    public async Task SignIn_WhenTwoFactorRequired_DoesNotSetCookie_AndReturnsPendingToken()
    {
        var request = new SignInRequest { Email = "juan@test.com", Password = "Password123!" };
        var expectedResponse = new AuthResponse
        {
            TwoFactorRequired = true,
            PendingTwoFactorToken = "pending-token",
            Role = "Client",
            UserId = Guid.NewGuid(),
            Email = "juan@test.com"
        };
        _authServiceMock.Setup(s => s.SignIn(request)).ReturnsAsync(expectedResponse);

        var result = await _controller.SignIn(request);

        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var body = Assert.IsType<AuthResponse>(okResult.Value);
        Assert.True(body.TwoFactorRequired);
        Assert.Equal("pending-token", body.PendingTwoFactorToken);
        Assert.DoesNotContain("access_token", _controller.Response.Headers.SetCookie.ToString());
        _authServiceMock.Verify(s => s.SignIn(request), Times.Once);
    }

    [Fact]
    public async Task VerifyTwoFactor_ReturnsOk_WithAuthResponse_TokenSetAsCookie()
    {
        var request = new TwoFactorVerifyRequest { PendingToken = "pending-token", Code = "123456" };
        var expectedResponse = new AuthResponse
        {
            Token = "jwt-token",
            Role = "Client",
            UserId = Guid.NewGuid(),
            Email = "juan@test.com"
        };
        _authServiceMock.Setup(s => s.VerifyTwoFactor(request.PendingToken, request.Code)).ReturnsAsync(expectedResponse);

        var result = await _controller.VerifyTwoFactor(request);

        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var body = Assert.IsType<AuthResponse>(okResult.Value);
        Assert.Equal(string.Empty, body.Token);
        Assert.Contains("access_token", _controller.Response.Headers.SetCookie.ToString());
        _authServiceMock.Verify(s => s.VerifyTwoFactor(request.PendingToken, request.Code), Times.Once);
    }
}
