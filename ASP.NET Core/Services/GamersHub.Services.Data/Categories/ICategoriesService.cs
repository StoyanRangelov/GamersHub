using System.Collections.Generic;
using System.Threading.Tasks;

namespace GamersHub.Services.Data.Categories
{
    public interface ICategoriesService
    {
        /// <summary>
        /// Returns a number of categories from the database, based on the provided take and skip values
        /// </summary>
        /// <param name="take"></param>
        /// <param name="skip"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        IEnumerable<T> GetAll<T>(int? take = null, int skip = 0);

        /// <summary>
        /// Returns all forum categories that does not have the given forum id
        /// </summary>
        /// <param name="id"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        IEnumerable<T> GetAllMissingByForumId<T>(int id);

        /// <summary>
        /// Returns a category by the specified id, returns null if such a category does not exist
        /// </summary>
        /// <param name="id"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        T GetById<T>(int id);

        /// <summary>
        /// Creates a new category with the given name and description and adds it to the database
        /// </summary>
        /// <param name="name"></param>
        /// <param name="description"></param>
        /// <returns></returns>
        Task<int> CreateAsync(string name, string description);

        /// <summary>
        /// Edits a category by the given id and updates it with the given name, description and forums to include in it, returns null if the category does not exist, returns 0 if the category name is already taken, otherwise if successful returns the category id
        /// </summary>
        /// <param name="id"></param>
        /// <param name="name"></param>
        /// <param name="description"></param>
        /// <param name="forumIds"></param>
        /// <param name="areSelected"></param>
        /// <returns></returns>

        Task<int?> EditAsync(int id, string name, string description, int[] forumIds, bool[] areSelected);

        /// <summary>
        /// Deletes the category with the given id and all of its relations, returns null if the category does not exist, returns category id if successful
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>

        Task<int?> DeleteAsync(int id);

        /// <summary>
        /// Returns the normalised version of the provided category name after comparing it to all other category names through the UrlParser
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>

        public string GetNormalisedName(string name);


        /// <summary>
        /// Returns the count of all categories in the database
        /// </summary>
        /// <returns></returns>
        int GetCount();
    }
}