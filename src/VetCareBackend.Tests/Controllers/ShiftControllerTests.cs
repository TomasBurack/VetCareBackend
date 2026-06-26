using Microsoft.AspNetCore.Mvc;
using Moq;
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

    public ShiftControllerTests()
    {
        _shiftServiceMock = new Mock<IShiftService>();
        _controller = new ShiftController(_shiftServiceMock.Object);
    }

    [Fact]
    public async Task Create_ReturnsOk_WithShiftResponse()
    {
        var request = new ShiftRequest
        {
            DateShift = DateTime.UtcNow.AddDays(1),
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
        _shiftServiceMock.Setup(s => s.Create(request)).ReturnsAsync(expected);

        var result = await _controller.Create(request);

        var ok = Assert.IsType<OkObjectResult>(result);
        Assert.Equal(expected, ok.Value);
        _shiftServiceMock.Verify(s => s.Create(request), Times.Once);
    }

    [Fact]
    public async Task GetAll_ReturnsOk_WithShiftList()
    {
        var shifts = new List<ShiftResponse>
        {
            new() { Description = "Turno 1", Status = "Pendient" },
            new() { Description = "Turno 2", Status = "Served" }
        };
        _shiftServiceMock.Setup(s => s.GetAll()).ReturnsAsync(shifts);

        var result = await _controller.GetAll();

        var ok = Assert.IsType<OkObjectResult>(result);
        Assert.Equal(shifts, ok.Value);
        _shiftServiceMock.Verify(s => s.GetAll(), Times.Once);
    }

    [Fact]
    public async Task UpdateStatus_ReturnsNoContent()
    {
        var shiftId = Guid.NewGuid();
        var request = new ShiftStatusRequest { Status = Status.Served };
        _shiftServiceMock.Setup(s => s.UpdateStatus(shiftId, request)).Returns(Task.CompletedTask);

        var result = await _controller.UpdateStatus(shiftId, request);

        Assert.IsType<NoContentResult>(result);
        _shiftServiceMock.Verify(s => s.UpdateStatus(shiftId, request), Times.Once);
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
