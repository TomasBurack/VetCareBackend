using Moq;
using VetCareBackend.Application.dtos.Requests;
using VetCareBackend.Application.Exceptions;
using VetCareBackend.Application.Infrastructure;
using VetCareBackend.Application.Interfaces;
using VetCareBackend.Application.Services;
using VetCareBackend.Domain.Entities;
using VetCareBackend.Domain.Enums;

namespace VetCareBackend.Tests.Services;

public class PetServiceTests
{
    private readonly Mock<IPetRepository> _petRepositoryMock;
    private readonly Mock<IClientRepository> _clientRepositoryMock;
    private readonly Mock<IBreedService> _breedServiceMock;
    private readonly PetService _petService;
    private readonly Guid _clientId = Guid.NewGuid();

    public PetServiceTests()
    {
        _petRepositoryMock = new Mock<IPetRepository>();
        _clientRepositoryMock = new Mock<IClientRepository>();
        _breedServiceMock = new Mock<IBreedService>();
        _petService = new PetService(_petRepositoryMock.Object, _clientRepositoryMock.Object, _breedServiceMock.Object);

        _clientRepositoryMock.Setup(r => r.Get(_clientId)).ReturnsAsync(new Client { Id = _clientId });
    }

    [Fact]
    public async Task Create_Succeeds_WhenBreedIsInAvailableList()
    {
        var request = new PetRequest { Name = "Firulais", Age = 2, typePet = TypePet.Canine, Breed = "Labrador" };
        _breedServiceMock.Setup(s => s.GetBreedsByTypeAsync(TypePet.Canine))
            .ReturnsAsync(new List<string> { "Labrador", "Poodle" });

        var result = await _petService.Create(request, _clientId.ToString());

        Assert.Equal("Labrador", result.Breed);
        _petRepositoryMock.Verify(r => r.Add(It.IsAny<Pet>()), Times.Once);
    }

    [Fact]
    public async Task Create_Throws_WhenBreedIsNotInAvailableList()
    {
        var request = new PetRequest { Name = "Firulais", Age = 2, typePet = TypePet.Canine, Breed = "Chihuahua" };
        _breedServiceMock.Setup(s => s.GetBreedsByTypeAsync(TypePet.Canine))
            .ReturnsAsync(new List<string> { "Labrador", "Poodle" });

        await Assert.ThrowsAsync<ValidationException>(() => _petService.Create(request, _clientId.ToString()));
        _petRepositoryMock.Verify(r => r.Add(It.IsAny<Pet>()), Times.Never);
    }

    [Fact]
    public async Task Create_Succeeds_WhenBreedListIsEmpty()
    {
        var request = new PetRequest { Name = "Rex", Age = 1, typePet = TypePet.Reptile, Breed = "Iguana" };
        _breedServiceMock.Setup(s => s.GetBreedsByTypeAsync(TypePet.Reptile))
            .ReturnsAsync(new List<string>());

        var result = await _petService.Create(request, _clientId.ToString());

        Assert.Equal("Iguana", result.Breed);
        _petRepositoryMock.Verify(r => r.Add(It.IsAny<Pet>()), Times.Once);
    }

    [Fact]
    public async Task Update_Throws_WhenBreedIsNotInAvailableList()
    {
        var petId = Guid.NewGuid();
        var existingPet = new Pet { Id = petId, IdClient = _clientId, Name = "Firulais", Age = 2, TypePet = TypePet.Canine, Breed = "Labrador" };
        _petRepositoryMock.Setup(r => r.Get(petId)).ReturnsAsync(existingPet);

        var request = new PetRequest { Name = "Firulais", Age = 3, typePet = TypePet.Canine, Breed = "Chihuahua" };
        _breedServiceMock.Setup(s => s.GetBreedsByTypeAsync(TypePet.Canine))
            .ReturnsAsync(new List<string> { "Labrador", "Poodle" });

        await Assert.ThrowsAsync<ValidationException>(() => _petService.Update(request, petId, _clientId.ToString()));
        _petRepositoryMock.Verify(r => r.Update(It.IsAny<Pet>()), Times.Never);
    }
}
