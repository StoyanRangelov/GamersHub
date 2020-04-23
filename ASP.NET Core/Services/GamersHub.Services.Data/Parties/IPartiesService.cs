namespace GamersHub.Services.Data.Parties
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using GamersHub.Data.Models;

    public interface IPartiesService
    {
        /// <summary>
        /// Returns a party from the database with the given id, returns null if such a party does not exist
        /// </summary>
        /// <param name="id"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        T GetById<T>(int id);

        /// <summary>
        /// Returns a number of parties, based on the provided take and skip values
        /// </summary>
        /// <param name="searchString"></param>
        /// <param name="take"></param>
        /// <param name="skip"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        IEnumerable<T> GetAll<T>(int? take = null, int skip = 0, string searchString = null);

        /// <summary>
        /// Returns the top five parties based on their party applications count
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        IEnumerable<T> GetTopFive<T>();

        /// <summary>
        /// Returns a number of parties with the given username, based on the given take and skip values
        /// </summary>
        /// <param name="username"></param>
        /// <param name="take"></param>
        /// <param name="skip"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        IEnumerable<T> GetAllByUsername<T>(string username, int? take = null, int skip = 0);

        /// <summary>
        /// Returns the count of all parties
        /// </summary>
        /// <param name="searchString"></param>
        /// <returns></returns>
        int GetCount(string searchString = null);

        /// <summary>
        /// Returns the count of all parties with the given username
        /// </summary>
        /// <param name="username"></param>
        /// <returns></returns>
        int GetCountByUsername(string username);

        /// <summary>
        /// Creates a party with the given user id, game, activity, description and size and adds it to the database
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="game"></param>
        /// <param name="activity"></param>
        /// <param name="description"></param>
        /// <param name="size"></param>
        /// <returns></returns>
        Task<int> CreateAsync(string userId, string game, ActivityType activity, string description, int size);

        /// <summary>
        /// Creates a party application by the given party id and user id, returns null if the party does not exists, returns 0 if the party application already exists, otherwise returns the party id
        /// </summary>
        /// <param name="partyId"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        Task<int?> ApplyAsync(int partyId, string userId);

        /// <summary>
        /// Edits a party by the given id and updates it with the given game, activity and description, returns null if the party does not exist, otherwise returns the party id
        /// </summary>
        /// <param name="id"></param>
        /// <param name="game"></param>
        /// <param name="activity"></param>
        /// <param name="description"></param>
        /// <returns></returns>
        Task<int?> EditAsync(int id, string game, ActivityType activity, string description);

        /// <summary>
        /// Deletes a party by the given id and all of its relations, returns null if the party does not exist, otherwise returns the party id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<int?> DeleteAsync(int id);
    }
}
