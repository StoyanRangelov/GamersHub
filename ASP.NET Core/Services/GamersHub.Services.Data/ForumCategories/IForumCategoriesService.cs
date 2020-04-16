using System.Collections.Generic;
using System.Threading.Tasks;

namespace GamersHub.Services.Data.ForumCategories
{
    public interface IForumCategoriesService
    {

        /// <summary>
        /// Returns all forum categories from the database with the given category name and forum id
        /// </summary>
        /// <param name="name"></param>
        /// <param name="id"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        T GetByNameAndForumId<T>(string name, int id);

        /// <summary>
        /// Returns all forum categories that do not have the given category id
        /// </summary>
        /// <param name="id"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        IEnumerable<T> GetAllMissingByCategoryId<T>(int id);
    }
}
