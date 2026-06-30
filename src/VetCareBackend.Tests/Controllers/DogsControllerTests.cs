using Moq;
using VetCareBackend.Application.Interfaces;
using VetCareBackend.Presentation.Controllers;

namespace VetCareBackend.Tests.Controllers;

public class DogsControllerTests
{
    private readonly Mock<IDogApiService> _dogServiceMock;
    private readonly DogsController _controller;

    public DogsControllerTests()
    {
        _dogServiceMock = new Mock<IDogApiService>();
        _controller = new DogsController(_dogServiceMock.Object);
    }

    [Fact]
    public async Task GetBreeds_ReturnsListOfBreeds()
    {
        var breeds = new List<string> { "Labrador", "Poodle", "Bulldog" }.AsReadOnly();
        _dogServiceMock.Setup(s => s.GetAllBreedsAsync()).ReturnsAsync(breeds);

        var result = await _controller.GetBreeds();

        Assert.NotNull(result);
        Assert.Equal(3, result.Count);
        Assert.Contains("Labrador", result);
        _dogServiceMock.Verify(s => s.GetAllBreedsAsync(), Times.Once);
    }

    [Fact]
    public async Task GetBreeds_ReturnsEmptyList_WhenNoBreeds()
    {
        var breeds = new List<string>().AsReadOnly();
        _dogServiceMock.Setup(s => s.GetAllBreedsAsync()).ReturnsAsync(breeds);

        var result = await _controller.GetBreeds();

        Assert.NotNull(result);
        Assert.Empty(result);
    }
}
