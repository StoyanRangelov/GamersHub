using System.Collections.Generic;
using System.Threading.Tasks;

namespace GamersHub.Services.Data.Forums
{
    public interface IForumsService
    {
        IEnumerable<T> GetAll<T>(int? count = null);

        T GetById<T>(int id);

        Task AddForumCategoryIfCategoryDoesNotExist(int forumId, int categoryId);

        Task<int> CreateAsync(string name);

        Task DeleteAsync(int id);
    }
}
