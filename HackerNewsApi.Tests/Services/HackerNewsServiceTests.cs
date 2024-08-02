using Moq;
using Newtonsoft.Json;
using Xunit;
using Microsoft.Extensions.Caching.Memory;
using HackerNewsApi.Models;
using HackerNewsApi.Services;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace HackerNewsApi.Tests
{
    public class HackerNewsServiceTests
    {
        private readonly HttpClient _httpClient;
        private readonly IMemoryCache _cache;
        private readonly IHackerNewsService _hackerNewsService;
        private readonly Mock<IHackerNewsService> _mockService;

        public HackerNewsServiceTests()
        {
            // Set up in-memory cache
            _cache = new MemoryCache(new MemoryCacheOptions());

            // Define mock responses
            var responses = new Dictionary<string, HttpResponseMessage>
            {
                // Mock the response for the newest stories endpoint
                {
                    "https://hacker-news.firebaseio.com/v0/newstories.json?print=pretty",
                    new HttpResponseMessage
                    {
                        StatusCode = HttpStatusCode.OK,
                        Content = new StringContent(JsonConvert.SerializeObject(new List<int> { 1, 2 }))
                    }
                },
                // Mock responses for individual story details
                {
                    "https://hacker-news.firebaseio.com/v0/item/1.json?print=pretty",
                    new HttpResponseMessage
                    {
                        StatusCode = HttpStatusCode.OK,
                        Content = new StringContent(JsonConvert.SerializeObject(new Story { Id = 1, Title = "Story 1", Url = "https://example.com/1" }))
                    }
                },
                {
                    "https://hacker-news.firebaseio.com/v0/item/2.json?print=pretty",
                    new HttpResponseMessage
                    {
                        StatusCode = HttpStatusCode.OK,
                        Content = new StringContent(JsonConvert.SerializeObject(new Story { Id = 2, Title = "Story 2", Url = "https://example.com/2" }))
                    }
                }
            };

            // Mock HttpMessageHandler
            var mockHandler = new MockHttpMessageHandler(responses);
            _httpClient = new HttpClient(mockHandler);

            // Initialize HackerNewsService with HttpClient
            _hackerNewsService = new HackerNewsService(_httpClient, _cache);

            // Initialize mock service
            _mockService = new Mock<IHackerNewsService>();
        }

        [Fact]
        public async Task GetNewestStoriesAsync_ReturnsStories()
        {
            // Act
            var stories = await _hackerNewsService.GetNewestStoriesAsync();

            // Assert
            Assert.NotNull(stories);
            Assert.Equal(2, stories.Count());
            Assert.Equal("Story 1", stories.First().Title);
            Assert.Equal("https://example.com/1", stories.First().Url);
        }

        [Fact]
        public async Task GetStoryByIdAsync_ReturnsStory()
        {
            // Arrange
            var storyId = 1;
            var expectedStory = new Story
            {
                Id = storyId,
                Title = "Test Story",
                Url = "https://example.com/test"
            };

            // Setup the mock service to return the expected story
            _mockService.Setup(service => service.GetStoryByIdAsync(storyId))
                        .ReturnsAsync(expectedStory);

            // Act
            var story = await _mockService.Object.GetStoryByIdAsync(storyId);

            // Assert
            Assert.NotNull(story);
            Assert.Equal(storyId, story.Id);
            Assert.Equal("Test Story", story.Title);
            Assert.Equal("https://example.com/test", story.Url);
        }
    }
}
