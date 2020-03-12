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
        private readonly IDeletableEntityRepository<ApplicationUser> usersRepository;
        private readonly IDeletableEntityRepository<Forum> forumsRepository;
        private readonly IDeletableEntityRepository<Category> categoriesRepository;

        public PostsService(
            IDeletableEntityRepository<Post> postsRepository,
            IDeletableEntityRepository<ApplicationUser> usersRepository,
            IDeletableEntityRepository<Forum> forumsRepository,
            IDeletableEntityRepository<Category> categoriesRepository)
        {
            this.postsRepository = postsRepository;
            this.usersRepository = usersRepository;
            this.forumsRepository = forumsRepository;
            this.categoriesRepository = categoriesRepository;
        }

        public async Task Create(string forumName, string categoryName, string topic, string content, string username)
        {
            var user = this.usersRepository.All().FirstOrDefault(x => x.UserName == username);
            var userId = user.Id;

            var forum = this.forumsRepository.All().FirstOrDefault(x => x.Name == forumName);
            var forumId = forum.Id;

            var category = this.categoriesRepository.All().FirstOrDefault(x => x.Name == categoryName);
            var categoryId = category.Id;

            var post = new Post
            {
                CreatedOn = DateTime.UtcNow,
                Topic = topic,
                Content = content,
                UserId = userId,
                User = user,
                ForumId = forumId,
                Forum = forum,
                CategoryId = categoryId,
                Category = category,
            };

            this.postsRepository.AddAsync(post).GetAwaiter().GetResult();
            this.postsRepository.SaveChangesAsync().GetAwaiter().GetResult();
        }
    }
}
