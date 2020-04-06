using System.Collections.Generic;
using System.Threading.Tasks;

namespace GamersHub.Services.Data.Users
{
    public interface IUsersService
    {
        IEnumerable<T> GetAllPromotableUsers<T>();

        IEnumerable<T> GetAllBannedUsers<T>();

        IEnumerable<T> GetAllAdministrators<T>();

        IEnumerable<T> GetAllModerators<T>();

        IEnumerable<T> GetTopFive<T>();

        IEnumerable<T> GetTopFiveBanned<T>();

        T GetById<T>(string id);

        Task PromoteAsync(string id, string role);

        Task BanAsync(string id);

        Task UnbanAsync(string id);
    }
}