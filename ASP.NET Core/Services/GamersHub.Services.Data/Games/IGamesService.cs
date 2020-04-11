using System.Collections.Generic;
using System.Threading.Tasks;

namespace GamersHub.Services.Data.Games
{
    public interface IGamesService
    {
        string GetUrl(int id);

        T GetById<T>(int id);

        IEnumerable<T> GetAll<T>(int? take = null, int skip = 0);

        IEnumerable<T> GetTopFive<T>();

        Task<int> CreateAsync(string title, string subTitle, string description, string imageUrl);

        Task<int> EditAsync(int id, string title, string subTitle, string description, string imageUrl);


        Task DeleteAsync(int id);

        int GetCount();
    }
}