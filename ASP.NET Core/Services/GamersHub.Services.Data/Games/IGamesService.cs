using System.Collections.Generic;
using System.Threading.Tasks;

namespace GamersHub.Services.Data.Games
{
    public interface IGamesService
    {
        IEnumerable<T> GetAll<T>();
        
        Task<int> CreateAsync(string title, string subTitle, string description, string imageUrl);
    }
}