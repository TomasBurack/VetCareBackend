using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System.Security.Claims;
using VetCareBackend.Application.dtos.Requests;
using VetCareBackend.Application.dtos.Responses;
using VetCareBackend.Application.Interfaces;
using VetCareBackend.Domain.Enums;
using VetCareBackend.Presentation.Controllers;

namespace VetCareBackend.Tests.Controllers;

public class ShiftControllerTests
{
    private readonly Mock<IShiftService> _shiftServiceMock;
    private readonly ShiftController _controller;
    private const string UserId = "auth0|client123";

    public ShiftControllerTests()
    {
        _shiftServiceMock = new Mock<IShiftService>();
        _controller = new ShiftController(_shiftServiceMock.Object);

        var claims = new List<Claim> { new Claim(ClaimTypes.NameIdentifier, UserId) };
        var identity = new ClaimsIdentity(claims);
        var principal = new ClaimsPrincipal(identity);
        _controller.ControllerContext = new ControllerContext
        {
            HttpContext = new DefaultHttpContext { User = principal }
        };
    }

    [Fact]
    public async Task Create_ReturnsOk_WithShiftResponse()
    {
        var request = new ShiftRequest
        {
            DateShift = DateTimeOffset.UtcNow.AddDays(1),
            Description = "Consulta general",
            PetId = Guid.NewGuid(),
            Enrollment = "VET001"
        };
        var expected = new ShiftResponse
        {
            DateShift = request.DateShift,
            Description = "Consulta general",
            Status = "Pendient",
            Enrollment = "VET001"
        };
        _shiftServiceMock.Setup(s => s.Create(request, UserId)).ReturnsAsync(expected);

        var result = await _controller.Create(request);

        var ok = Assert.IsType<OkObjectResult>(result);
        Assert.Equal(expected, ok.Value);
        _shiftServiceMock.Verify(s => s.Create(request, UserId), Times.Once);
    }

    [Fact]
    public async Task GetAllAdmin_ReturnsOk_WithShiftList()
    {
        var shifts = new List<ShiftResponse>
        {
            new() { Description = "Turno 1", Status = "Pendient" },
            new() { Description = "Turno 2", Status = "Served" }
        };
        _shiftServiceMock
            .Setup(s => s.GetAllAdmin())
            .ReturnsAsync(shifts);

        var result = await _controller.GetAllAdmin();

        var ok = Assert.IsType<OkObjectResult>(result);
        Assert.Equal(shifts, ok.Value);
        _shiftServiceMock.Verify(s => s.GetAllAdmin(), Times.Once);
    }

    [Fact]
    public async Task GetAllClient_ReturnsOk_WithShiftList()
    {
       
        var clientId = Guid.NewGuid().ToString();
        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, clientId)
        };
        var identity = new ClaimsIdentity(claims);
        var principal = new ClaimsPrincipal(identity);
        _controller.ControllerContext = new ControllerContext
        {
            HttpContext = new DefaultHttpContext { User = principal }
        };

        var shifts = new List<ShiftResponse>
        {
            new() { Description = "Turno mascota 1", Status = "Pendient" }
        };
        _shiftServiceMock
            .Setup(s => s.GetAllClient(clientId))
            .ReturnsAsync(shifts);

        var result = await _controller.GetAllClient();

        var ok = Assert.IsType<OkObjectResult>(result);
        Assert.Equal(shifts, ok.Value);
        _shiftServiceMock.Verify(s => s.GetAllClient(clientId), Times.Once);
    }

    [Fact]
    public async Task GetAllVeterinarian_ReturnsOk_WithShiftList()
    {
        
        var vetId = Guid.NewGuid().ToString();
        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, vetId)
        };
        var identity = new ClaimsIdentity(claims);
        var principal = new ClaimsPrincipal(identity);
        _controller.ControllerContext = new ControllerContext
        {
            HttpContext = new DefaultHttpContext { User = principal }
        };

        var shifts = new List<ShiftResponse>
        {
            new() { Description = "Turno vet 1", Status = "Pendient" }
        };
        _shiftServiceMock
            .Setup(s => s.GetAllVeterinarian(vetId))
            .ReturnsAsync(shifts);

        var result = await _controller.GetAllVeterinarian();

        var ok = Assert.IsType<OkObjectResult>(result);
        Assert.Equal(shifts, ok.Value);
        _shiftServiceMock.Verify(s => s.GetAllVeterinarian(vetId), Times.Once);
    }

    [Fact]
    public async Task UpdateStatusClient_ReturnsNoContent()
    {
        var shiftId = Guid.NewGuid();
        _shiftServiceMock.Setup(s => s.CancelStatusClient(shiftId, UserId)).Returns(Task.CompletedTask);

        var result = await _controller.UpdateStatusClient(shiftId);

        Assert.IsType<NoContentResult>(result);
        _shiftServiceMock.Verify(s => s.CancelStatusClient(shiftId, UserId), Times.Once);
    }

    [Fact]
    public async Task UpdateStatusVeterinarian_ReturnsNoContent()
    {
        var shiftId = Guid.NewGuid();
        var request = new ShiftStatusRequest { Status = Status.Served };
        _shiftServiceMock.Setup(s => s.UpdateStatusVeterinarian(shiftId, request, UserId)).Returns(Task.CompletedTask);

        var result = await _controller.UpdateStatusVeterinarian(shiftId, request);

        Assert.IsType<NoContentResult>(result);
        _shiftServiceMock.Verify(s => s.UpdateStatusVeterinarian(shiftId, request, UserId), Times.Once);
    }

    [Fact]
    public async Task Delete_ReturnsNoContent()
    {
        var shiftId = Guid.NewGuid();
        _shiftServiceMock.Setup(s => s.Delete(shiftId)).Returns(Task.CompletedTask);

        var result = await _controller.Delete(shiftId);

        Assert.IsType<NoContentResult>(result);
        _shiftServiceMock.Verify(s => s.Delete(shiftId), Times.Once);
    }
}