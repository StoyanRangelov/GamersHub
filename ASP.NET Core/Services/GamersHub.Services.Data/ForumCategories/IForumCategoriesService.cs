using System.Threading.Tasks;

namespace GamersHub.Services.Data.ForumCategories
{
    public interface IForumCategoriesService
    {
        T GetByNameAndForumId<T>(string name, int id);

        Task DeleteAllByCategoryIdAsync(int id);
    }
}
