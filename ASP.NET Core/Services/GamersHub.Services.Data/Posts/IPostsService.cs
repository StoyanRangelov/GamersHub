using System.Collections.Generic;
using System.Threading.Tasks;

namespace GamersHub.Services.Data.Posts
{
    public interface IPostsService
    {
        T GetByName<T>(string name);

        T GetById<T>(int id);

        IEnumerable<T> GetAll<T>(int? take = null, int skip = 0);

        IEnumerable<T> GetTopFive<T>();

        IEnumerable<T> GetAllByForumId<T>(int forumId, int? take = null, int skip = 0);

        IEnumerable<T> GetAllByCategoryNameAndForumId<T>(string name, int id);

        Task<int> CreateAsync(int forumId, int categoryId, string name, string content, string userId);

        Task<int> EditAsync(int id, string name, string content);

        Task DeleteAsync(int id);

        int GetCountByForumId(int forumId);

        int GetCount();
    }
}