using System.Threading.Tasks;

namespace GamersHub.Services.Data
{
    public interface IPostsService
    {
        T GetById<T>(int id);

        Task CreateAsync(string forumName, string categoryName, string topic, string content, string userId);
    }
}