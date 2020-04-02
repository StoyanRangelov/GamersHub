using System.Threading.Tasks;

namespace GamersHub.Services.Data.Replies
{
    public interface IRepliesService
    {
        T GetById<T>(int id);

        Task<int> CreateAsync(int postId, string userId, string content);

        Task<int> EditAsync(int id, string content);

        Task DeleteAsync(int id);
    }
}