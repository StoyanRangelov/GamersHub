namespace GamersHub.Services.Data.ForumCategories
{
    using System.Linq;

    using GamersHub.Data.Common.Repositories;
    using GamersHub.Data.Models;
    using GamersHub.Services.Mapping;

    public class ForumCategoriesService : IForumCategoriesService
    {
        private readonly IRepository<ForumCategory> forumCategoriesRepository;

        public ForumCategoriesService(IRepository<ForumCategory> forumCategoriesRepository)
        {
            this.forumCategoriesRepository = forumCategoriesRepository;
        }

        public T GetByCategoryNameAndForumId<T>(string categoryName, int forumId)
        {
            var forumCategory = this.forumCategoriesRepository.All()
                .Where(x => x.Category.Name == categoryName && x.ForumId == forumId)
                .To<T>().FirstOrDefault();

            return forumCategory;
        }
    }
}
