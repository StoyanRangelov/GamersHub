using System.Collections.Generic;
using System.Threading.Tasks;

namespace GamersHub.Services.Data.Reviews
{
    public interface IReviewsService
    {
        /// <summary>
        /// Returns a number of reviews with the given game id, based on the given take na skip values
        /// </summary>
        /// <param name="gameId"></param>
        /// <param name="take"></param>
        /// <param name="skip"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        IEnumerable<T> GetAllByGameId<T>(int gameId, int? take = null, int skip = 0);


        /// <summary>
        /// Returns a review by the given id, returns null if such a review does not exist
        /// </summary>
        /// <param name="id"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        T GetById<T>(int id);

        /// <summary>
        /// Creates a review with the given game id, is positive value, content and user id and adds it to the database
        /// </summary>
        /// <param name="gameId"></param>
        /// <param name="isPositive"></param>
        /// <param name="content"></param>
        /// <param name="userId"></param>
        /// <returns></returns>

        Task<int> CreateAsync(int gameId, bool isPositive, string content, string userId);

        /// <summary>
        /// Edits a review with the given id and updates it with the given content and is positive value, returns null if such a review does not exist, otherwise returns the review id
        /// </summary>
        /// <param name="id"></param>
        /// <param name="content"></param>
        /// <param name="isPositive"></param>
        /// <returns></returns>

        Task<int?> EditAsync(int id, string content, bool isPositive);
        
        /// <summary>
        /// Deletes the review with the given id, returns null if such a review does not exist, otherwise returns the review id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>

        Task<int?> DeleteAsync(int id);
        
        /// <summary>
        /// returns the count of all reviews with the given game id from the database
        /// </summary>
        /// <param name="gameId"></param>
        /// <returns></returns>

        int GetCountByGameId(int gameId);
    }
}