using Microsoft.AspNetCore.Mvc;
using Moq;
using VetCareBackend.Application.Interfaces;
using VetCareBackend.Presentation.Controllers;

namespace VetCareBackend.Tests.Controllers;

public class CatsControllerTests
{
    private readonly Mock<ICatApiService> _catServiceMock;
    private readonly CatsController _controller;

    public CatsControllerTests()
    {
        _catServiceMock = new Mock<ICatApiService>();
        _controller = new CatsController(_catServiceMock.Object);
    }

    [Fact]
    public async Task GetBreeds_ReturnsListOfBreeds()
    {
        var breeds = new List<string> { "Persian", "Siamese", "Maine Coon" }.AsReadOnly();
        _catServiceMock.Setup(s => s.GetAllBreedsAsync()).ReturnsAsync(breeds);

        var result = await _controller.GetBreeds();

        Assert.NotNull(result);
        Assert.Equal(3, result.Count);
        Assert.Contains("Persian", result);
        _catServiceMock.Verify(s => s.GetAllBreedsAsync(), Times.Once);
    }

    [Fact]
    public async Task GetBreeds_ReturnsEmptyList_WhenNoBreeds()
    {
        var breeds = new List<string>().AsReadOnly();
        _catServiceMock.Setup(s => s.GetAllBreedsAsync()).ReturnsAsync(breeds);

        var result = await _controller.GetBreeds();

        Assert.NotNull(result);
        Assert.Empty(result);
    }
}
