using System.Threading.Tasks;

namespace GamersHub.Services.Data
{
    public interface IPostsService
    {
        T GetByName<T>(string name);
        
        Task CreateAsync(string forumName, string categoryName, string topic, string content, string userId);
    }
}