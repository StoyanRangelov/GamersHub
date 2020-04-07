using System.Threading.Tasks;

namespace GamersHub.Services.Data.Reviews
{
    public interface IReviewsService
    {
        Task<int> CreateAsync(int gameId, bool isPositive, string content, string userId);
    }
}