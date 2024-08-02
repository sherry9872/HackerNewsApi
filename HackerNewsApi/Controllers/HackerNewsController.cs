using HackerNewsApi.Models;
using HackerNewsApi.Services;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace HackerNewsApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class HackerNewsController : ControllerBase
    {
        private readonly IHackerNewsService _hackerNewsService;

        public HackerNewsController(IHackerNewsService hackerNewsService)
        {
            _hackerNewsService = hackerNewsService;
        }

        [HttpGet("stories")]
        public async Task<IActionResult> GetNewestStories()
        {
            var stories = await _hackerNewsService.GetNewestStoriesAsync();
            return Ok(stories);
        }

        [HttpGet("story/{id}")]
        public async Task<IActionResult> GetStoryById(int id)
        {
            var story = await _hackerNewsService.GetStoryByIdAsync(id);
            if (story == null)
            {
                return NotFound();
            }
            return Ok(story);
        }
    }
}
