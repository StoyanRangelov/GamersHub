using System.Collections.Generic;
using System.Threading.Tasks;

namespace GamersHub.Services.Data.Replies
{
    public interface IRepliesService
    {
        IEnumerable<T> GetAllByPostId<T>(int postId, int? take = null, int skip = 0);

        T GetById<T>(int id);

        Task<int> CreateAsync(int postId, string userId, string content);

        Task<int> EditAsync(int id, string content);

        Task DeleteAsync(int id);

        int GetCountByForumId(int postId);
    }
}