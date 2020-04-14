using System.Collections.Generic;
using System.Threading.Tasks;

namespace GamersHub.Services.Data.Parties
{
    public interface IPartiesService
    {
        T GetById<T>(int id);

        IEnumerable<T> GetAll<T>(int? take = null, int skip = 0);

        IEnumerable<T> GetTopFive<T>();

        IEnumerable<T> GetAllByUsername<T>(string username, int? take = null, int skip = 0);

        IEnumerable<T> GetAllApplicationsByUsername<T>(string username, int? take = null, int skip = 0);

        int GetCount();

        int GetCountByUsername(string username);

        int GetApplicationsCountByUsername(string username);

        Task<int> CreateAsync(string userId, string game, string activity, string description, int size);

        Task<int> ApplyAsync(int partyId, string userId);

        Task<int> ApproveAsync(int partyId, string applicantId);

        Task<int> DeclineAsync(int partyId, string applicantId);

        Task<int> CancelApplicationAsync(int partyId, string applicantId);

        Task DeleteAsync(int id);
    }
}