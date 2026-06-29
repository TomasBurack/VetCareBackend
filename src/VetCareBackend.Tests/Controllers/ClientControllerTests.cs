using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using VetCareBackend.Application.dtos.Requests;
using VetCareBackend.Application.dtos.Responses;
using VetCareBackend.Application.Interfaces;
using VetCareBackend.Presentation.Controllers;
using VetCareBackend.Tests.Helpers;

namespace VetCareBackend.Tests.Controllers;

public class ClientControllerTests
{
    private readonly Mock<IClientService> _clientServiceMock;
    private readonly Mock<IHttpContextAccessor> _httpContextAccessorMock;
    private readonly ClientController _controller;
    private const string UserId = "auth0|client123";

    public ClientControllerTests()
    {
        _clientServiceMock = new Mock<IClientService>();
        _httpContextAccessorMock = new Mock<IHttpContextAccessor>();
        _controller = new ClientController(_httpContextAccessorMock.Object, _clientServiceMock.Object);
        ControllerTestHelper.SetUserClaim(_controller, UserId);
    }

    [Fact]
    public async Task Get_MyUser_ReturnsOk_WithClientResponse()
    {
        var expected = new ClientResponse { Id = Guid.NewGuid(), FirstName = "Maria", Email = "maria@test.com" };
        _clientServiceMock.Setup(s => s.Get(UserId)).ReturnsAsync(expected);

        var result = await _controller.Get();

        var ok = Assert.IsType<OkObjectResult>(result);
        Assert.Equal(expected, ok.Value);
        _clientServiceMock.Verify(s => s.Get(UserId), Times.Once);
    }

    [Fact]
    public async Task Delete_MyUser_ReturnsNoContent()
    {
        _clientServiceMock.Setup(s => s.Delete(UserId)).Returns(Task.CompletedTask);

        var result = await _controller.Delete();

        Assert.IsType<NoContentResult>(result);
        _clientServiceMock.Verify(s => s.Delete(UserId), Times.Once);
    }

    [Fact]
    public async Task Update_MyUser_ReturnsNoContent()
    {
        var request = new UserRequest { FirstName = "Maria", LastName = "Lopez" };
        _clientServiceMock.Setup(s => s.Update(UserId, request)).Returns(Task.CompletedTask);

        var result = await _controller.Update(request);

        Assert.IsType<NoContentResult>(result);
        _clientServiceMock.Verify(s => s.Update(UserId, request), Times.Once);
    }

    [Fact]
    public async Task Get_ById_ReturnsOk_WithClientResponse()
    {
        var clientId = "auth0|clientxyz";
        var expected = new ClientResponse { Id = Guid.NewGuid(), FirstName = "Pedro" };
        _clientServiceMock.Setup(s => s.Get(clientId)).ReturnsAsync(expected);

        var result = await _controller.Get(clientId);

        var ok = Assert.IsType<OkObjectResult>(result);
        Assert.Equal(expected, ok.Value);
    }

    [Fact]
    public async Task Create_ReturnsOk_WithUserResponse()
    {
        var request = new SignUpRequest { FirstName = "Nuevo", Email = "nuevo@test.com", Password = "Pass123!" };
        var expected = new UserResponse { Id = Guid.NewGuid(), FirstName = "Nuevo" };
        _clientServiceMock.Setup(s => s.Create(request)).ReturnsAsync(expected);

        var result = await _controller.Create(request);

        var ok = Assert.IsType<OkObjectResult>(result);
        Assert.Equal(expected, ok.Value);
    }

    [Fact]
    public async Task Delete_ById_ReturnsNoContent()
    {
        var clientId = "auth0|clientxyz";
        _clientServiceMock.Setup(s => s.Delete(clientId)).Returns(Task.CompletedTask);

        var result = await _controller.Delete(clientId);

        Assert.IsType<NoContentResult>(result);
        _clientServiceMock.Verify(s => s.Delete(clientId), Times.Once);
    }

    [Fact]
    public async Task Update_ById_ReturnsNoContent()
    {
        var clientId = "auth0|clientxyz";
        var request = new UserRequest { FirstName = "Updated" };
        _clientServiceMock.Setup(s => s.Update(clientId, request)).Returns(Task.CompletedTask);

        var result = await _controller.Update(request, clientId);

        Assert.IsType<NoContentResult>(result);
    }

    [Fact]
    public async Task GetAll_ReturnsOk_WithClientList()
    {
        var clients = new List<UserResponse>
        {
            new() { Id = Guid.NewGuid(), FirstName = "Client1" },
            new() { Id = Guid.NewGuid(), FirstName = "Client2" }
        };
        _clientServiceMock.Setup(s => s.GetAll()).ReturnsAsync(clients);

        var result = await _controller.GetAll();

        var ok = Assert.IsType<OkObjectResult>(result);
        Assert.Equal(clients, ok.Value);
    }
}
