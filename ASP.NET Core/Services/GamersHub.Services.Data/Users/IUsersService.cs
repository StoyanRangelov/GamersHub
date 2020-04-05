using System.Collections.Generic;
using System.Threading.Tasks;

namespace GamersHub.Services.Data.Users
{
    public interface IUsersService
    {
        IEnumerable<T> GetAllPromotableUsers<T>();

        IEnumerable<T> GetAllAdministrators<T>();

        IEnumerable<T> GetAllModerators<T>();

        IEnumerable<T> GetTopFive<T>();

        T GetById<T>(string id);
    }
}