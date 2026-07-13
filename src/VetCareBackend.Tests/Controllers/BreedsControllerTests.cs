using Moq;
using VetCareBackend.Application.Interfaces;
using VetCareBackend.Domain.Enums;
using VetCareBackend.Presentation.Controllers;

namespace VetCareBackend.Tests.Controllers;

public class BreedsControllerTests
{
    private readonly Mock<IBreedService> _breedServiceMock;
    private readonly BreedsController _controller;

    public BreedsControllerTests()
    {
        _breedServiceMock = new Mock<IBreedService>();
        _controller = new BreedsController(_breedServiceMock.Object);
    }

    [Fact]
    public async Task GetBreeds_ReturnsListOfBreeds_ForFeline()
    {
        var breeds = new List<string> { "Persian", "Siamese", "Maine Coon" }.AsReadOnly();
        _breedServiceMock.Setup(s => s.GetBreedsByTypeAsync(TypePet.Feline)).ReturnsAsync(breeds);

        var result = await _controller.GetBreeds(TypePet.Feline);

        Assert.NotNull(result);
        Assert.Equal(3, result.Count);
        Assert.Contains("Persian", result);
        _breedServiceMock.Verify(s => s.GetBreedsByTypeAsync(TypePet.Feline), Times.Once);
    }

    [Fact]
    public async Task GetBreeds_ReturnsListOfBreeds_ForCanine()
    {
        var breeds = new List<string> { "Labrador", "Poodle", "Bulldog" }.AsReadOnly();
        _breedServiceMock.Setup(s => s.GetBreedsByTypeAsync(TypePet.Canine)).ReturnsAsync(breeds);

        var result = await _controller.GetBreeds(TypePet.Canine);

        Assert.NotNull(result);
        Assert.Equal(3, result.Count);
        Assert.Contains("Labrador", result);
        _breedServiceMock.Verify(s => s.GetBreedsByTypeAsync(TypePet.Canine), Times.Once);
    }

    [Fact]
    public async Task GetBreeds_ReturnsEmptyList_WhenNoBreeds()
    {
        var breeds = new List<string>().AsReadOnly();
        _breedServiceMock.Setup(s => s.GetBreedsByTypeAsync(TypePet.Reptile)).ReturnsAsync(breeds);

        var result = await _controller.GetBreeds(TypePet.Reptile);

        Assert.NotNull(result);
        Assert.Empty(result);
    }
}
