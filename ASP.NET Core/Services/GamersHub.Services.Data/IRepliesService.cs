using System.Threading.Tasks;

namespace GamersHub.Services.Data
{
    public interface IRepliesService
    {
        Task<int> CreateAsync(int postId, string userId, string content);
    }
}