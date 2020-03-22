using System.Collections.Generic;
using System.Threading.Tasks;

namespace GamersHub.Services.Data
{
    public interface ICategoriesService
    {
        IEnumerable<T> GetAll<T>(int? count = null);

        IEnumerable<string> GetAllNames();

        Task CreateAsync(string name, string description);

        bool CheckIfExistsByName(string name);
    }
}