using Moq;
using VetCareBackend.Application.Interfaces;
using VetCareBackend.Domain.Enums;
using VetCareBackend.Infrastructure.ExternalService;

namespace VetCareBackend.Tests.Services;

public class BreedServiceTests
{
    private readonly Mock<ICatApiService> _catServiceMock;
    private readonly Mock<IDogApiService> _dogServiceMock;
    private readonly BreedService _breedService;

    public BreedServiceTests()
    {
        _catServiceMock = new Mock<ICatApiService>();
        _dogServiceMock = new Mock<IDogApiService>();
        _breedService = new BreedService(_catServiceMock.Object, _dogServiceMock.Object);
    }

    [Fact]
    public async Task GetBreedsByTypeAsync_ReturnsCatBreeds_ForFeline()
    {
        var breeds = new List<string> { "Persian", "Siamese" }.AsReadOnly();
        _catServiceMock.Setup(s => s.GetAllBreedsAsync()).ReturnsAsync(breeds);

        var result = await _breedService.GetBreedsByTypeAsync(TypePet.Feline);

        Assert.Equal(breeds, result);
        _catServiceMock.Verify(s => s.GetAllBreedsAsync(), Times.Once);
        _dogServiceMock.Verify(s => s.GetAllBreedsAsync(), Times.Never);
    }

    [Fact]
    public async Task GetBreedsByTypeAsync_ReturnsDogBreeds_ForCanine()
    {
        var breeds = new List<string> { "Labrador", "Poodle" }.AsReadOnly();
        _dogServiceMock.Setup(s => s.GetAllBreedsAsync()).ReturnsAsync(breeds);

        var result = await _breedService.GetBreedsByTypeAsync(TypePet.Canine);

        Assert.Equal(breeds, result);
        _dogServiceMock.Verify(s => s.GetAllBreedsAsync(), Times.Once);
        _catServiceMock.Verify(s => s.GetAllBreedsAsync(), Times.Never);
    }

    [Theory]
    [InlineData(TypePet.Reptile)]
    [InlineData(TypePet.Avian)]
    public async Task GetBreedsByTypeAsync_ReturnsEmptyList_ForUnsupportedTypes(TypePet typePet)
    {
        var result = await _breedService.GetBreedsByTypeAsync(typePet);

        Assert.Empty(result);
        _catServiceMock.Verify(s => s.GetAllBreedsAsync(), Times.Never);
        _dogServiceMock.Verify(s => s.GetAllBreedsAsync(), Times.Never);
    }
}
