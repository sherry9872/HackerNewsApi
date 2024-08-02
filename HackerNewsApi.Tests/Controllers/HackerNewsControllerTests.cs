using Moq;
using Newtonsoft.Json;
using Xunit;
using Microsoft.AspNetCore.Mvc;
using HackerNewsApi.Controllers;
using HackerNewsApi.Models;
using HackerNewsApi.Services;
using System.Threading.Tasks;

namespace HackerNewsApi.Tests
{
    public class HackerNewsControllerTests
    {
        private readonly Mock<IHackerNewsService> _mockService;
        private readonly HackerNewsController _controller;

        public HackerNewsControllerTests()
        {
            _mockService = new Mock<IHackerNewsService>();
            _controller = new HackerNewsController(_mockService.Object);
        }

        [Fact]
        public async Task GetStoryById_ReturnsStory_WhenStoryExists()
        {
            // Arrange
            var storyId = 1;
            var expectedStory = new Story
            {
                Id = storyId,
                Title = "Test Story",
                Url = "https://example.com/test"
            };
            _mockService.Setup(service => service.GetStoryByIdAsync(storyId))
                        .ReturnsAsync(expectedStory);

            // Act
            var result = await _controller.GetStoryById(storyId) as OkObjectResult;

            // Assert
            Assert.NotNull(result);
            Assert.IsType<Story>(result.Value);
            var story = result.Value as Story;
            Assert.Equal(storyId, story.Id);
            Assert.Equal("Test Story", story.Title);
            Assert.Equal("https://example.com/test", story.Url);
        }

        [Fact]
        public async Task GetStoryById_ReturnsNotFound_WhenStoryDoesNotExist()
        {
            // Arrange
            var storyId = 999;
            _mockService.Setup(service => service.GetStoryByIdAsync(storyId))
                        .ReturnsAsync((Story)null);

            // Act
            var result = await _controller.GetStoryById(storyId);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }
    }
}
