using System.Collections.Generic;
using System.Threading.Tasks;

namespace GamersHub.Services.Data.PartyApplicants
{
    public interface IPartyApplicantsService
    {
        /// <summary>
        /// Approves a party application to the party by the given party id and applicant id, returns null if the party application does not exist, otherwise returns the party id
        /// </summary>
        /// <param name="partyId"></param>
        /// <param name="applicantId"></param>
        /// <returns></returns>
        Task<int?> ApproveAsync(int partyId, string applicantId);

        /// <summary>
        /// Declines a party application from a party by the given party id and applicant id, returns null if the party application does not exist, otherwise returns the party id
        /// </summary>
        /// <param name="partyId"></param>
        /// <param name="applicantId"></param>
        /// <returns></returns>
        Task<int?> DeclineAsync(int partyId, string applicantId);

        /// <summary>
        /// Removes a party application from a party by the given party id and applicant id, returns null if the party does not exist, otherwise returns the party id
        /// </summary>
        /// <param name="partyId"></param>
        /// <param name="applicantId"></param>
        /// <returns></returns>
        Task<int?> CancelApplicationAsync(int partyId, string applicantId);

        /// <summary>
        /// Returns a number of party applications with the given username, based on the given take and skip values
        /// </summary>
        /// <param name="username"></param>
        /// <param name="take"></param>
        /// <param name="skip"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        IEnumerable<T> GetAllApplicationsByUsername<T>(string username, int? take = null, int skip = 0);

        /// <summary>
        /// Returns the count of all party applications with the given username
        /// </summary>
        /// <param name="username"></param>
        /// <returns></returns>
        int GetApplicationsCountByUsername(string username);
    }
}