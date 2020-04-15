using System.Collections.Generic;
using System.Threading.Tasks;
using GamersHub.Data.Models;

namespace GamersHub.Services.Data.Users
{
    public interface IUsersService
    {
        /// <summary>
        /// Returns a number of users not in Administrator or Moderator roles, by the given take and skip values
        /// </summary>
        /// <param name="take"></param>
        /// <param name="skip"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        IEnumerable<T> GetAllPromotableUsers<T>(int? take = null, int skip = 0);

        /// <summary>
        /// Returns a number of users whose Lockout end is not null, by the given take and skip values
        /// </summary>
        /// <param name="take"></param>
        /// <param name="skip"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>

        IEnumerable<T> GetAllBannedUsers<T>(int? take = null, int skip = 0);

        /// <summary>
        /// Returns all users int the given role
        /// </summary>
        /// <param name="role"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>

        IEnumerable<T> GetAllByRole<T>(string role);

        /// <summary>
        /// Returns the top five users based on the provided order type
        /// </summary>
        /// <param name="orderType"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>

        IEnumerable<T> GetTopFive<T>(string orderType = null);

        /// <summary>
        /// Returns the user with the given id, returns null if the user does not exist in the database
        /// </summary>
        /// <param name="id"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        T GetById<T>(string id);
        
        /// <summary>
        /// Retursn the user with the given name, returns null if the user does not exist in the database
        /// </summary>
        /// <param name="name"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>

        T GetByName<T>(string name);

        /// <summary>
        /// Promotes the user with the given id to the given role
        /// </summary>
        /// <param name="id"></param>
        /// <param name="role"></param>
        /// <returns></returns>

        Task PromoteAsync(string id, string role);

        /// <summary>
        /// Removes the user with the given id from the Moderator role
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>

        Task DemoteAsync(string id);

        /// <summary>
        /// Sets the Lockout end of the user with the given id to 30 days
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>

        Task BanAsync(string id);

        /// <summary>
        /// Sets the Lockout end of the user with the given id to null
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>

        Task UnbanAsync(string id);

        /// <summary>
        /// Deletes the user with the given id, removes him from all roles and sets his username, email address and discord username to null, returns false if the user does not exists in the database
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>

        Task<bool> DeleteAsync(string id);

        /// <summary>
        /// Returns the count of all users from the database who are not in the Administrator and Moderator roles
        /// </summary>
        /// <returns></returns>

        int GetCountOfPromotableUsers();

        /// <summary>
        /// Returns the count of all users from the database whose Lockout end is not null
        /// </summary>
        /// <returns></returns>

        int GetCountOfBannedUsers();

        /// <summary>
        /// Returns the id of the user by the given name
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>

        string GetIdByName(string name);
    }
}