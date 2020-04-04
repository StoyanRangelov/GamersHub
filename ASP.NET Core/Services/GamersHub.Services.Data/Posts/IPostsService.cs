using System.Collections.Generic;
using System.Threading.Tasks;

namespace GamersHub.Services.Data.Posts
{
    public interface IPostsService
    {
        T GetById<T>(int id);

        IEnumerable<T> GetAll<T>();

        IEnumerable<T> GetAllByCategoryNameAndForumId<T>(string name, int id);

        Task<int> CreateAsync(int forumId, int categoryId, string name, string content, string userId);

        Task<int> EditAsync(int id, string name, string content);

        Task DeleteAsync(int id);
    }
}
