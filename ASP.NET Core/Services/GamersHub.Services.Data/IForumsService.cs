using System.Collections.Generic;
using System.Threading.Tasks;
using GamersHub.Data.Models;

namespace GamersHub.Services.Data
{
    public interface IForumsService
    {
        IEnumerable<T> GetAll<T>(int? count = null);

        T GetByUrl<T>(string url);

        Task<int> CreateAsync(string name);
    }
}
