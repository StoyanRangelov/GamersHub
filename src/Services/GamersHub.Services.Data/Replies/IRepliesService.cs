namespace GamersHub.Services.Data.Replies
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface IRepliesService
    {
        /// <summary>
        /// Returns a number of replies with the given post id from the database, based on the provided take and skip values
        /// </summary>
        /// <param name="postId"></param>
        /// <param name="take"></param>
        /// <param name="skip"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        IEnumerable<T> GetAllByPostId<T>(int postId, int? take = null, int skip = 0);

        /// <summary>
        /// Returns a reply with the given id, returns null if the reply does not exist in the database
        /// </summary>
        /// <param name="id"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        T GetById<T>(int id);

        /// <summary>
        /// Creates a reply with the given post id, user id and content and adds it to the database
        /// </summary>
        /// <param name="postId"></param>
        /// <param name="userId"></param>
        /// <param name="content"></param>
        /// <returns></returns>
        Task<int> CreateAsync(int postId, string userId, string content);

        /// <summary>
        /// Edits a reply with the given id and updates it with the given content, returns null if the reply does not exist, otherwise returns the reply id
        /// </summary>
        /// <param name="id"></param>
        /// <param name="content"></param>
        /// <returns></returns>
        Task<int?> EditAsync(int id, string content);

        /// <summary>
        /// Deletes the reply with the given id, returns null if the reply does not exist, otherwise returns the reply id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<int?> DeleteAsync(int id);

        /// <summary>
        /// Returns the count of all replies with the given post id from the database
        /// </summary>
        /// <param name="postId"></param>
        /// <returns></returns>
        int GetCountByPostId(int postId);
    }
}
