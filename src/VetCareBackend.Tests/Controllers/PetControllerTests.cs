using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using VetCareBackend.Application.dtos.Requests;
using VetCareBackend.Application.dtos.Responses;
using VetCareBackend.Application.Interfaces;
using VetCareBackend.Domain.Enums;
using VetCareBackend.Presentation.Controllers;
using VetCareBackend.Tests.Helpers;

namespace VetCareBackend.Tests.Controllers;

public class PetControllerTests
{
    private readonly Mock<IPetService> _petServiceMock;
    private readonly Mock<IHttpContextAccessor> _httpContextAccessorMock;
    private readonly PetController _controller;
    private const string UserId = "auth0|client123";

    public PetControllerTests()
    {
        _petServiceMock = new Mock<IPetService>();
        _httpContextAccessorMock = new Mock<IHttpContextAccessor>();
        _controller = new PetController(_petServiceMock.Object, _httpContextAccessorMock.Object);
        ControllerTestHelper.SetUserClaim(_controller, UserId);
    }

    [Fact]
    public async Task GetAll_ReturnsOk_WhenPetsExist()
    {
        var pets = new List<PetResponse>
        {
            new() { IdPet = Guid.NewGuid(), Name = "Firulais", TypePet = TypePet.Canine },
            new() { IdPet = Guid.NewGuid(), Name = "Michi", TypePet = TypePet.Feline }
        };
        _petServiceMock.Setup(s => s.GetAll(UserId)).ReturnsAsync(pets);

        var result = await _controller.GetAll();

        var ok = Assert.IsType<OkObjectResult>(result.Result);
        var body = Assert.IsType<List<PetResponse>>(ok.Value);
        Assert.Equal(2, body.Count);
        _petServiceMock.Verify(s => s.GetAll(UserId), Times.Once);
    }

    [Fact]
    public async Task GetAll_ReturnsNotFound_WhenNoPets()
    {
        _petServiceMock.Setup(s => s.GetAll(UserId)).ReturnsAsync(new List<PetResponse>());

        var result = await _controller.GetAll();

        var notFound = Assert.IsType<NotFoundObjectResult>(result.Result);
        Assert.Equal("No ha mascotas registradas.", notFound.Value);
    }

    [Fact]
    public async Task GetById_ReturnsOk_WithPetResponse()
    {
        var petId = Guid.NewGuid();
        var expected = new PetResponse { IdPet = petId, Name = "Firulais", TypePet = TypePet.Canine };
        _petServiceMock.Setup(s => s.GetById(petId, UserId)).ReturnsAsync(expected);

        var result = await _controller.GetById(petId);

        var ok = Assert.IsType<OkObjectResult>(result.Result);
        var body = Assert.IsType<PetResponse>(ok.Value);
        Assert.Equal(petId, body.IdPet);
        _petServiceMock.Verify(s => s.GetById(petId, UserId), Times.Once);
    }

    [Fact]
    public async Task Create_ReturnsCreatedAtAction_WithPetResponse()
    {
        var request = new PetRequest { Name = "Firulais", Age = 3, typePet = TypePet.Canine, Breed = "Labrador" };
        var created = new PetResponse { IdPet = Guid.NewGuid(), Name = "Firulais", TypePet = TypePet.Canine };
        _petServiceMock.Setup(s => s.Create(request, UserId)).ReturnsAsync(created);

        var result = await _controller.Create(request);

        var createdAt = Assert.IsType<CreatedAtActionResult>(result.Result);
        Assert.Equal(nameof(_controller.GetById), createdAt.ActionName);
        Assert.Equal(created.IdPet, createdAt.RouteValues!["id"]);
        var body = Assert.IsType<PetResponse>(createdAt.Value);
        Assert.Equal("Firulais", body.Name);
        _petServiceMock.Verify(s => s.Create(request, UserId), Times.Once);
    }

    [Fact]
    public async Task Delete_ReturnsNoContent()
    {
        var petId = Guid.NewGuid();
        _petServiceMock.Setup(s => s.Delete(petId, UserId)).Returns(Task.CompletedTask);

        var result = await _controller.Delete(petId);

        Assert.IsType<NoContentResult>(result);
        _petServiceMock.Verify(s => s.Delete(petId, UserId), Times.Once);
    }

    [Fact]
    public async Task Update_ReturnsNoContent()
    {
        var petId = Guid.NewGuid();
        var request = new PetRequest { Name = "Firulais Updated", Age = 4, typePet = TypePet.Canine };
        _petServiceMock.Setup(s => s.Update(request, petId, UserId)).Returns(Task.CompletedTask);

        var result = await _controller.Update(request, petId);

        Assert.IsType<NoContentResult>(result);
        _petServiceMock.Verify(s => s.Update(request, petId, UserId), Times.Once);
    }
}
