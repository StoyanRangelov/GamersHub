using System.Threading.Tasks;

namespace GamersHub.Services.Data
{
    public interface IPostsService
    {
        Task Create(string forumName, string categoryName, string topic, string content, string username);
    }
}