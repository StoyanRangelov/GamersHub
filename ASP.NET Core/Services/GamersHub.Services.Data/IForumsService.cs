using System.Collections.Generic;
using System.Threading.Tasks;

namespace GamersHub.Services.Data
{
    public interface IForumsService
    {
        IEnumerable<T> GetAll<T>(int? count = null);

        T GetByName<T>(string name);

        T GetById<T>(int id);

        Task CreateAsync(string name);

        bool CheckIfExistsByName(string name);
    }
}