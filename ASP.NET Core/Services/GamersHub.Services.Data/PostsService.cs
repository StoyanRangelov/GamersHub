using System;
using System.Linq;
using System.Threading.Tasks;
using GamersHub.Data.Common.Repositories;
using GamersHub.Data.Models;

namespace GamersHub.Services.Data
{
    public class PostsService : IPostsService
    {
        private readonly IDeletableEntityRepository<Post> postsRepository;

        private readonly IForumsService forumsService;
        private readonly ICategoriesService categoriesService;

        public PostsService(
            IDeletableEntityRepository<Post> postsRepository,
            IForumsService forumsService,
            ICategoriesService categoriesService)
        {
            this.postsRepository = postsRepository;
            this.forumsService = forumsService;
            this.categoriesService = categoriesService;
        }

        public async Task CreateAsync(string forumName, string categoryName, string name, string content, string userId)
        {
            var forumId = this.forumsService.GetIdByName(forumName);
            var categoryId = this.categoriesService.GetIdByName(categoryName);

            var post = new Post
            {
                CreatedOn = DateTime.UtcNow,
                Topic = name,
                Content = content,
                UserId = userId,
                ForumId = forumId,
                CategoryId = categoryId,
            };

            await this.postsRepository.AddAsync(post);
            await this.postsRepository.SaveChangesAsync();
        }
    }
}
