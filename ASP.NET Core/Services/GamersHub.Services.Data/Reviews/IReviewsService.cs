using System.Threading.Tasks;

namespace GamersHub.Services.Data.Reviews
{
    public interface IReviewsService
    {
        T GetById<T>(int id);

        Task<int> CreateAsync(int gameId, bool isPositive, string content, string userId);

        Task<int> EditAsync(int id, string content, bool isPositive);
    }
}