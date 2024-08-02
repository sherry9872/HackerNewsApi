using HackerNewsApi.Models;
using Microsoft.Extensions.Caching.Memory;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace HackerNewsApi.Services
{
    public class HackerNewsService : IHackerNewsService
    {
        private readonly HttpClient _httpClient;
        private readonly IMemoryCache _cache;
        private const string NewStoriesUrl = "https://hacker-news.firebaseio.com/v0/newstories.json?print=pretty";
        private const string ItemUrl = "https://hacker-news.firebaseio.com/v0/item/{0}.json?print=pretty";

        public HackerNewsService(HttpClient httpClient, IMemoryCache cache)
        {
            _httpClient = httpClient;
            _cache = cache;
        }

        public async Task<IEnumerable<Story>> GetNewestStoriesAsync()
        {
            var storyIds = await _httpClient.GetStringAsync(NewStoriesUrl);
            var ids = JsonConvert.DeserializeObject<List<int>>(storyIds);
            
            var tasks = ids.Take(10).Select(async id => 
            {
                var url = string.Format(ItemUrl, id);
                var response = await _httpClient.GetStringAsync(url);
                return JsonConvert.DeserializeObject<Story>(response);
            });

            var stories = await Task.WhenAll(tasks);
            return stories;
        }

        public async Task<Story> GetStoryByIdAsync(int id)
        {
            var url = string.Format(ItemUrl, id);
            var response = await _httpClient.GetStringAsync(url);
            return JsonConvert.DeserializeObject<Story>(response);
        }
    }
}
