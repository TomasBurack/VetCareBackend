using Microsoft.AspNetCore.Mvc;
using Moq;
using VetCareBackend.Application.dtos.Requests;
using VetCareBackend.Application.dtos.Responses;
using VetCareBackend.Application.Interfaces;
using VetCareBackend.Domain.Enums;
using VetCareBackend.Presentation.Controllers;
using VetCareBackend.Tests.Helpers;

namespace VetCareBackend.Tests.Controllers;

public class VeterinarianControllerTests
{
    private readonly Mock<IVeterinarianService> _vetServiceMock;
    private readonly VeterinarianController _controller;
    private const string UserId = "auth0|vet123";

    public VeterinarianControllerTests()
    {
        _vetServiceMock = new Mock<IVeterinarianService>();
        _controller = new VeterinarianController(_vetServiceMock.Object);
        ControllerTestHelper.SetUserClaim(_controller, UserId);
    }

    [Fact]
    public async Task Create_ReturnsOk_WithVeterinarianResponse()
    {
        var request = new VeterinarianRequest
        {
            FirstName = "Dr. Juan",
            LastName = "Vet",
            Email = "vet@test.com",
            Enrollment = "VET001",
            Speciality = Speciality.Common
        };
        var expected = new VeterinarianResponse { Id = Guid.NewGuid(), FirstName = "Dr. Juan", Enrollment = "VET001" };
        _vetServiceMock.Setup(s => s.Create(request)).ReturnsAsync(expected);

        var result = await _controller.Create(request);

        var ok = Assert.IsType<OkObjectResult>(result);
        Assert.Equal(expected, ok.Value);
        _vetServiceMock.Verify(s => s.Create(request), Times.Once);
    }

    [Fact]
    public async Task GetById_ReturnsOk_WithVeterinarianResponse()
    {
        var vetId = "auth0|vetxyz";
        var expected = new VeterinarianResponse { Id = Guid.NewGuid(), FirstName = "Dr. Ana", Enrollment = "VET002" };
        _vetServiceMock.Setup(s => s.GetById(vetId)).ReturnsAsync(expected);

        var result = await _controller.GetById(vetId);

        var ok = Assert.IsType<OkObjectResult>(result);
        Assert.Equal(expected, ok.Value);
        _vetServiceMock.Verify(s => s.GetById(vetId), Times.Once);
    }

    [Fact]
    public async Task UpdateByAdmin_ReturnsNoContent()
    {
        var vetId = "auth0|vetxyz";
        var request = new VeterinarianUpdateRequest { FirstName = "Updated", Speciality = Speciality.Surgery };
        _vetServiceMock.Setup(s => s.Update(vetId, request)).Returns(Task.CompletedTask);

        var result = await _controller.UpdateByAdmin(vetId, request);

        Assert.IsType<NoContentResult>(result);
        _vetServiceMock.Verify(s => s.Update(vetId, request), Times.Once);
    }

    [Fact]
    public async Task AdminDelete_ReturnsNoContent()
    {
        var vetId = "auth0|vetxyz";
        _vetServiceMock.Setup(s => s.Delete(vetId)).Returns(Task.CompletedTask);

        var result = await _controller.AdminDelete(vetId);

        Assert.IsType<NoContentResult>(result);
        _vetServiceMock.Verify(s => s.Delete(vetId), Times.Once);
    }

    [Fact]
    public async Task GetAll_ReturnsOk_WithVeterinarianList()
    {
        var vets = new List<VeterinarianResponse>
        {
            new() { Id = Guid.NewGuid(), FirstName = "Vet1", Enrollment = "VET001" },
            new() { Id = Guid.NewGuid(), FirstName = "Vet2", Enrollment = "VET002" }
        };
        _vetServiceMock.Setup(s => s.GetAll()).ReturnsAsync(vets);

        var result = await _controller.GetAll();

        var ok = Assert.IsType<OkObjectResult>(result);
        Assert.Equal(vets, ok.Value);
        _vetServiceMock.Verify(s => s.GetAll(), Times.Once);
    }

    [Fact]
    public async Task Get_MyUser_ReturnsOk_WithVeterinarianResponse()
    {
        var expected = new VeterinarianResponse { Id = Guid.NewGuid(), FirstName = "Dr. Juan", Enrollment = "VET001" };
        _vetServiceMock.Setup(s => s.GetById(UserId)).ReturnsAsync(expected);

        var result = await _controller.Get();

        var ok = Assert.IsType<OkObjectResult>(result);
        Assert.Equal(expected, ok.Value);
        _vetServiceMock.Verify(s => s.GetById(UserId), Times.Once);
    }

    [Fact]
    public async Task Update_MyUser_ReturnsNoContent()
    {
        var request = new VeterinarianUpdateRequest { FirstName = "Updated", Speciality = Speciality.Cardiology };
        _vetServiceMock.Setup(s => s.Update(UserId, request)).Returns(Task.CompletedTask);

        var result = await _controller.Update(request);

        Assert.IsType<NoContentResult>(result);
        _vetServiceMock.Verify(s => s.Update(UserId, request), Times.Once);
    }

    [Fact]
    public async Task Delete_MyUser_ReturnsNoContent()
    {
        _vetServiceMock.Setup(s => s.Delete(UserId)).Returns(Task.CompletedTask);

        var result = await _controller.Delete();

        Assert.IsType<NoContentResult>(result);
        _vetServiceMock.Verify(s => s.Delete(UserId), Times.Once);
    }
}
