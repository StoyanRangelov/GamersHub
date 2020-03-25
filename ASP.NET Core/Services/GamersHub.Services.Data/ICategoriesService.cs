using System.Collections.Generic;
using System.Threading.Tasks;
using GamersHub.Data.Models;

namespace GamersHub.Services.Data
{
    public interface ICategoriesService
    {
        string GetNameByUrl(string url);

        IEnumerable<T> GetAll<T>(int? count = null);

        IEnumerable<string> GetAllNames();

        Task<int> CreateAsync(string name, string description);

        bool CheckIfExistsByName(string name);
    }
}