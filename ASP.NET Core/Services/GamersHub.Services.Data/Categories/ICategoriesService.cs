using System.Collections.Generic;
using System.Threading.Tasks;

namespace GamersHub.Services.Data.Categories
{
    public interface ICategoriesService
    {
        IEnumerable<T> GetAll<T>(int? take = null, int skip = 0);

        IEnumerable<T> GetAllByForumId<T>(int id);

        T GetById<T>(int id);

        Task<int> CreateAsync(string name, string description);

        Task<int> EditAsync(int id, string name, string description, int[] forumIds, bool[] areSelected);

        Task DeleteAsync(int id);

        public string GetNormalisedName(string name);

        int GetCount();
    }
}