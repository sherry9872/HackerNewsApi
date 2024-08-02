using HackerNewsApi.Models;
using System.Threading.Tasks;

namespace HackerNewsApi.Services
{
    public interface IHackerNewsService
    {
        Task<IEnumerable<Story>> GetNewestStoriesAsync();
        Task<Story> GetStoryByIdAsync(int id); // Add this line
    }
}
