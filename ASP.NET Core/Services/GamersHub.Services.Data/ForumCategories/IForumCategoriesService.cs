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
        T GetByCategoryNameAndForumId<T>(string name, int id);
    }
}
