using Microsoft.AspNetCore.Mvc;
using Moq;
using VetCareBackend.Application.dtos.Requests;
using VetCareBackend.Application.dtos.Responses;
using VetCareBackend.Application.Interfaces;
using VetCareBackend.Presentation.Controllers;
using VetCareBackend.Tests.Helpers;

namespace VetCareBackend.Tests.Controllers;

public class SysadminControllerTests
{
    private readonly Mock<ISysadminService> _sysadminServiceMock;
    private readonly SysadminController _controller;
    private const string UserId = "auth0|sysadmin123";

    public SysadminControllerTests()
    {
        _sysadminServiceMock = new Mock<ISysadminService>();
        _controller = new SysadminController(_sysadminServiceMock.Object);
        ControllerTestHelper.SetUserClaim(_controller, UserId);
    }

    [Fact]
    public async Task Get_MyUser_ReturnsOk_WithUserResponse()
    {
        var expected = new UserResponse { Id = Guid.NewGuid(), FirstName = "Sys", Email = "sys@test.com" };
        _sysadminServiceMock.Setup(s => s.Get(UserId)).ReturnsAsync(expected);

        var result = await _controller.Get();

        var ok = Assert.IsType<OkObjectResult>(result);
        Assert.Equal(expected, ok.Value);
        _sysadminServiceMock.Verify(s => s.Get(UserId), Times.Once);
    }

    [Fact]
    public async Task Delete_MyUser_ReturnsNoContent()
    {
        _sysadminServiceMock.Setup(s => s.Delete(UserId)).Returns(Task.CompletedTask);

        var result = await _controller.Delete();

        Assert.IsType<NoContentResult>(result);
        _sysadminServiceMock.Verify(s => s.Delete(UserId), Times.Once);
    }

    [Fact]
    public async Task Update_MyUser_ReturnsNoContent()
    {
        var request = new UserRequest { FirstName = "Updated", Email = "updated@test.com" };
        _sysadminServiceMock.Setup(s => s.Update(UserId, request)).Returns(Task.CompletedTask);

        var result = await _controller.Update(request);

        Assert.IsType<NoContentResult>(result);
        _sysadminServiceMock.Verify(s => s.Update(UserId, request), Times.Once);
    }

    [Fact]
    public async Task GetAll_ReturnsOk_WithSysadminList()
    {
        var sysadmins = new List<UserResponse>
        {
            new() { Id = Guid.NewGuid(), FirstName = "Sys1" },
            new() { Id = Guid.NewGuid(), FirstName = "Sys2" }
        };
        _sysadminServiceMock.Setup(s => s.GetAll()).ReturnsAsync(sysadmins);

        var result = await _controller.GetAll();

        var ok = Assert.IsType<OkObjectResult>(result);
        Assert.Equal(sysadmins, ok.Value);
    }
}
