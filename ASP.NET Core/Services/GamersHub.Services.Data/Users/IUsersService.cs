using System.Collections.Generic;
using System.Threading.Tasks;
using GamersHub.Data.Models;

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

        Task DemoteAsync(string id);

        Task BanAsync(string id);

        Task UnbanAsync(string id);

        Task<bool> ValidateUserCanEditDeleteById(string id, ApplicationUser user);
    }
}