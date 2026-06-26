using Microsoft.AspNetCore.Mvc;
using Moq;
using VetCareBackend.Application.dtos.Requests;
using VetCareBackend.Application.dtos.Responses;
using VetCareBackend.Application.Interfaces;
using VetCareBackend.Presentation.Controllers;
using VetCareBackend.Tests.Helpers;

namespace VetCareBackend.Tests.Controllers;

public class AdministratorControllerTests
{
    private readonly Mock<IAdministratorService> _adminServiceMock;
    private readonly Mock<IVeterinarianService> _vetServiceMock;
    private readonly Mock<IClientService> _clientServiceMock;
    private readonly Mock<ISysadminService> _sysadminServiceMock;
    private readonly AdministratorController _controller;
    private const string UserId = "auth0|abc123";

    public AdministratorControllerTests()
    {
        _adminServiceMock = new Mock<IAdministratorService>();
        _vetServiceMock = new Mock<IVeterinarianService>();
        _clientServiceMock = new Mock<IClientService>();
        _sysadminServiceMock = new Mock<ISysadminService>();
        _controller = new AdministratorController(
            _adminServiceMock.Object,
            _vetServiceMock.Object,
            _clientServiceMock.Object,
            _sysadminServiceMock.Object
        );
        ControllerTestHelper.SetUserClaim(_controller, UserId);
    }

    [Fact]
    public async Task Get_MyUser_ReturnsOk_WithUserResponse()
    {
        var expected = new UserResponse { Id = Guid.NewGuid(), FirstName = "Carlos", Email = "carlos@test.com" };
        _adminServiceMock.Setup(s => s.Get(UserId)).ReturnsAsync(expected);

        var result = await _controller.Get();

        var ok = Assert.IsType<OkObjectResult>(result);
        Assert.Equal(expected, ok.Value);
        _adminServiceMock.Verify(s => s.Get(UserId), Times.Once);
    }

    [Fact]
    public async Task Delete_MyUser_ReturnsNoContent()
    {
        _adminServiceMock.Setup(s => s.Delete(UserId)).Returns(Task.CompletedTask);

        var result = await _controller.Delete();

        Assert.IsType<NoContentResult>(result);
        _adminServiceMock.Verify(s => s.Delete(UserId), Times.Once);
    }

    [Fact]
    public async Task Update_MyUser_ReturnsNoContent()
    {
        var request = new UserRequest { FirstName = "Carlos", LastName = "Gomez", Email = "carlos@test.com" };
        _adminServiceMock.Setup(s => s.Update(UserId, request)).Returns(Task.CompletedTask);

        var result = await _controller.Update(request);

        Assert.IsType<NoContentResult>(result);
        _adminServiceMock.Verify(s => s.Update(UserId, request), Times.Once);
    }

    [Fact]
    public async Task Create_ReturnsOk_WithUserResponse()
    {
        var request = new SignUpRequest { FirstName = "Admin", Email = "admin@test.com", Password = "Pass123!" };
        var expected = new UserResponse { Id = Guid.NewGuid(), FirstName = "Admin" };
        _adminServiceMock.Setup(s => s.Create(request)).ReturnsAsync(expected);

        var result = await _controller.Create(request);

        var ok = Assert.IsType<OkObjectResult>(result);
        Assert.Equal(expected, ok.Value);
        _adminServiceMock.Verify(s => s.Create(request), Times.Once);
    }

    [Fact]
    public async Task Get_ById_ReturnsOk_WithUserResponse()
    {
        var adminId = "auth0|xyz789";
        var expected = new UserResponse { Id = Guid.NewGuid(), FirstName = "Admin2" };
        _adminServiceMock.Setup(s => s.Get(adminId)).ReturnsAsync(expected);

        var result = await _controller.Get(adminId);

        var ok = Assert.IsType<OkObjectResult>(result);
        Assert.Equal(expected, ok.Value);
    }

    [Fact]
    public async Task Delete_ById_ReturnsNoContent()
    {
        var adminId = "auth0|xyz789";
        _adminServiceMock.Setup(s => s.Delete(adminId)).Returns(Task.CompletedTask);

        var result = await _controller.Delete(adminId);

        Assert.IsType<NoContentResult>(result);
        _adminServiceMock.Verify(s => s.Delete(adminId), Times.Once);
    }

    [Fact]
    public async Task Update_ById_ReturnsNoContent()
    {
        var adminId = "auth0|xyz789";
        var request = new UserRequest { FirstName = "Updated" };
        _adminServiceMock.Setup(s => s.Update(adminId, request)).Returns(Task.CompletedTask);

        var result = await _controller.Update(request, adminId);

        Assert.IsType<NoContentResult>(result);
        _adminServiceMock.Verify(s => s.Update(adminId, request), Times.Once);
    }

    [Fact]
    public async Task GetAll_ReturnsOk_WithAdminList()
    {
        var admins = new List<UserResponse>
        {
            new() { Id = Guid.NewGuid(), FirstName = "Admin1" },
            new() { Id = Guid.NewGuid(), FirstName = "Admin2" }
        };
        _adminServiceMock.Setup(s => s.GetAll()).ReturnsAsync(admins);

        var result = await _controller.GetAll();

        var ok = Assert.IsType<OkObjectResult>(result);
        Assert.Equal(admins, ok.Value);
    }

    [Fact]
    public async Task GetAllUsers_ReturnsOk_WithCombinedUsers()
    {
        var admins = new List<UserResponse> { new() { FirstName = "Admin1" } };
        var clients = new List<UserResponse> { new() { FirstName = "Client1" } };
        var vets = new List<VeterinarianResponse> { new() { FirstName = "Vet1" } };
        var sysadmins = new List<UserResponse> { new() { FirstName = "Sys1" } };

        _adminServiceMock.Setup(s => s.GetAll()).ReturnsAsync(admins);
        _clientServiceMock.Setup(s => s.GetAll()).ReturnsAsync(clients);
        _vetServiceMock.Setup(s => s.GetAll()).ReturnsAsync(vets);
        _sysadminServiceMock.Setup(s => s.GetAll()).ReturnsAsync(sysadmins);

        var result = await _controller.GetAllUsers();

        var ok = Assert.IsType<OkObjectResult>(result);
        Assert.NotNull(ok.Value);
    }
}
