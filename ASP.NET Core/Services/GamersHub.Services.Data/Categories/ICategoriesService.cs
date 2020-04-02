using System.Collections.Generic;
using System.Threading.Tasks;

namespace GamersHub.Services.Data.Categories
{
    public interface ICategoriesService
    {
        IEnumerable<T> GetAll<T>(int? count = null);

        T GetById<T>(int id);

        Task<int> CreateAsync(string name, string description);

        Task DeleteAsync(int id);

        public string GetNormalisedName(string name);
    }
}