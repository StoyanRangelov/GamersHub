using System.Threading.Tasks;

namespace GamersHub.Services.Data
{
    public interface IRepliesService
    {
        T GetById<T>(int id);

        Task<int> CreateAsync(int postId, string userId, string content);

        Task<int> EditAsync(int id, string content);

        Task DeleteAsync(int id);
    }
}