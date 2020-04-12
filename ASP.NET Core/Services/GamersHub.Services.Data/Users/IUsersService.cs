using System.Collections.Generic;
using System.Threading.Tasks;
using GamersHub.Data.Models;

namespace GamersHub.Services.Data.Users
{
    public interface IUsersService
    {
        IEnumerable<T> GetAllPromotableUsers<T>(int? take = null, int skip = 0);

        IEnumerable<T> GetAllBannedUsers<T>(int? take = null, int skip = 0);

        IEnumerable<T> GetAllByRole<T>(string role);

        IEnumerable<T> GetTopFive<T>(string orderType = null);

        T GetById<T>(string id);

        T GetByName<T>(string name);

        Task PromoteAsync(string id, string role);

        Task DemoteAsync(string id);

        Task BanAsync(string id);

        Task UnbanAsync(string id);

        Task<bool> DeleteAsync(string id);

        int GetCountOfPromotableUsers();

        int GetCountOfBannedUsers();

        string GetIdByName(string name);
    }
}