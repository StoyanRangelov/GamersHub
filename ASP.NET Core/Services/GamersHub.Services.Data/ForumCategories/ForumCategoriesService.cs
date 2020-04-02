using System.Linq;
using System.Threading.Tasks;
using GamersHub.Data.Common.Repositories;
using GamersHub.Data.Models;
using GamersHub.Services.Mapping;

namespace GamersHub.Services.Data.ForumCategories
{
    public class ForumCategoriesService : IForumCategoriesService
    {
        private readonly IRepository<ForumCategory> forumCategoriesRepository;

        public ForumCategoriesService(IRepository<ForumCategory> forumCategoriesRepository)
        {
            this.forumCategoriesRepository = forumCategoriesRepository;
        }


        public T GetByNameAndForumId<T>(string name, int id)
        {
            var forumCategory = this.forumCategoriesRepository.All()
                .Where(x => x.Category.Name == name && x.ForumId == id)
                .To<T>().FirstOrDefault();

            return forumCategory;
        }

        public async Task DeleteAllByCategoryIdAsync(int id)
        {
            var categoryForums = this.forumCategoriesRepository.All()
                .Where(x => x.Category.Id == id).ToList();

            foreach (var categoryForum in categoryForums)
            {
                this.forumCategoriesRepository.Delete(categoryForum);
            }

            await this.forumCategoriesRepository.SaveChangesAsync();
        }
    }
}