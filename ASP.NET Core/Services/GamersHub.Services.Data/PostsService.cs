using System;
using System.Linq;
using System.Threading.Tasks;
using GamersHub.Data.Common.Repositories;
using GamersHub.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace GamersHub.Services.Data
{
    public class PostsService : IPostsService
    {
        private readonly IDeletableEntityRepository<Post> postsRepository;
        private readonly IDeletableEntityRepository<Forum> forumsRepository;
        private readonly IDeletableEntityRepository<Category> categoriesRepository;

        public PostsService(
            IDeletableEntityRepository<Post> postsRepository,
            IDeletableEntityRepository<Forum> forumsRepository,
            IDeletableEntityRepository<Category> categoriesRepository)
        {
            this.postsRepository = postsRepository;
            this.forumsRepository = forumsRepository;
            this.categoriesRepository = categoriesRepository;
        }

        //TODO : Implement Method
        public T GetByName<T>(string name)
        {
            throw new NotImplementedException();
        }

        public async Task CreateAsync(string forumName, string categoryName, string name, string content, string userId)
        {
            var forum = this.forumsRepository.All()
                .Include(x => x.ForumCategories)
                .First(x => x.Name == forumName);
            var category = this.categoriesRepository.All().First(x => x.Name == categoryName);

            if (!forum.ForumCategories.Select(fc => fc.CategoryId).Contains(category.Id))
            {
                forum.ForumCategories.Add(new ForumCategory
                {
                    ForumId = forum.Id,
                    CategoryId = category.Id,
                });

                this.forumsRepository.Update(forum);
                await this.forumsRepository.SaveChangesAsync();
            }

            var post = new Post
            {
                CreatedOn = DateTime.UtcNow,
                Name = name,
                Content = content,
                UserId = userId,
                ForumId = forum.Id,
                CategoryId = category.Id,
            };

            await this.postsRepository.AddAsync(post);
            await this.postsRepository.SaveChangesAsync();
        }
    }
}
