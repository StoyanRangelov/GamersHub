using System.Threading.Tasks;

namespace GamersHub.Services.Data.Games
{
    public interface IGamesService
    {
        Task<int> CreateAsync(string title, string subTitle, string description, string imageUrl);
    }
}