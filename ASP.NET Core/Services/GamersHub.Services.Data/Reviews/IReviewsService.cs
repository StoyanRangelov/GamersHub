using System.Collections.Generic;
using System.Threading.Tasks;

namespace GamersHub.Services.Data.Reviews
{
    public interface IReviewsService
    {
        IEnumerable<T> GetAllByGameId<T>(int gameId, int? take = null, int skip = 0);

        T GetById<T>(int id);

        Task<int> CreateAsync(int gameId, bool isPositive, string content, string userId);

        Task<int> EditAsync(int id, string content, bool isPositive);

        Task DeleteAsync(int id);

        int GetCountByGameId(int gameId);
    }
}