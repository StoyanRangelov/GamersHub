using System.Collections.Generic;
using System.Threading.Tasks;

namespace GamersHub.Services.Data.Posts
{
    public interface IPostsService
    {
        /// <summary>
        /// Returns a post with the given name, returns null if such a post does not exist in the database
        /// </summary>
        /// <param name="name"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        T GetByName<T>(string name);

        /// <summary>
        /// Returns a post with the given id, returns null if such a post does not exist in the database
        /// </summary>
        /// <param name="id"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        T GetById<T>(int id);

        /// <summary>
        /// Returns a number of posts, based on the provided take and skip values
        /// </summary>
        /// <param name="take"></param>
        /// <param name="skip"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>

        IEnumerable<T> GetAll<T>(int? take = null, int skip = 0);

        /// <summary>
        /// Returns the top five posts based on their reviews count
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>

        IEnumerable<T> GetTopFive<T>();
        
        /// <summary>
        /// Returns a number of posts with the given forum id, based on the given take and skip values
        /// </summary>
        /// <param name="forumId"></param>
        /// <param name="take"></param>
        /// <param name="skip"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>

        IEnumerable<T> GetAllByForumId<T>(int forumId, int? take = null, int skip = 0);
        
        /// <summary>
        /// Returns a number of posts with the given category name and forum id, based on the given take and skip values
        /// </summary>
        /// <param name="name"></param>
        /// <param name="id"></param>
        /// <param name="take"></param>
        /// <param name="skip"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>

        IEnumerable<T> GetAllByCategoryNameAndForumId<T>(string name, int id, int? take = null, int skip = 0);
        
        /// <summary>
        /// Creates a new post with the given forum id, category id, name, content and user id, returns 0 if the given forum does not exist, otherwise returns the post id
        /// </summary>
        /// <param name="forumId"></param>
        /// <param name="categoryId"></param>
        /// <param name="name"></param>
        /// <param name="content"></param>
        /// <param name="userId"></param>
        /// <returns></returns>

        Task<int> CreateAsync(int forumId, int categoryId, string name, string content, string userId);

        /// <summary>
        /// Edits the post with the given id and updates it with the given name and description, returns 0 if the post does not exist, otherwise returns the post id
        /// </summary>
        /// <param name="id"></param>
        /// <param name="name"></param>
        /// <param name="content"></param>
        /// <returns></returns>

        Task<int> EditAsync(int id, string name, string content);
        
        /// <summary>
        /// Deletes the post with the given id and all of its relations 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>

        Task DeleteAsync(int id);
        
        /// <summary>
        /// Returns the count of all posts with the given forum id
        /// </summary>
        /// <param name="forumId"></param>
        /// <returns></returns>

        int GetCountByForumId(int forumId);
        
        /// <summary>
        /// Returns the count of all posts in the database
        /// </summary>
        /// <returns></returns>

        int GetCount();
        
        /// <summary>
        /// Returns the count of all posts in the database with the given category name and forum id
        /// </summary>
        /// <param name="name"></param>
        /// <param name="forumId"></param>
        /// <returns></returns>

        int GetCountByCategoryNameAndForumId(string name, int forumId);
    }
}