using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GamersHub.Common;
using GamersHub.Data.Common.Repositories;
using GamersHub.Data.Models;
using GamersHub.Services.Data.ForumCategories;
using GamersHub.Services.Data.Posts;
using GamersHub.Services.Mapping;
using Microsoft.EntityFrameworkCore;

namespace GamersHub.Services.Data.Categories
{
    public class CategoriesService : ICategoriesService
    {
        private readonly IDeletableEntityRepository<Category> categoriesRepository;
        private readonly IDeletableEntityRepository<Post> postsRepository;
        private readonly IRepository<ForumCategory> forumCategoriesRepository;

        public CategoriesService(
            IDeletableEntityRepository<Category> categoriesRepository,
            IDeletableEntityRepository<Post> postsRepository,
            IRepository<ForumCategory> forumCategoriesRepository)
        {
            this.categoriesRepository = categoriesRepository;
            this.postsRepository = postsRepository;
            this.forumCategoriesRepository = forumCategoriesRepository;
        }

        public IEnumerable<T> GetAll<T>(int? count = null)
        {
            IQueryable<Category> query =
                this.categoriesRepository.All();
            if (count.HasValue)
            {
                query = query.Take(count.Value);
            }

            return query.To<T>().ToList();
        }

        public T GetById<T>(int id)
        {
            var category = this.categoriesRepository.All()
                .Where(x => x.Id == id)
                .To<T>().FirstOrDefault();

            return category;
        }

        public async Task<int> CreateAsync(string name, string description)
        {
            var alreadyExists = this.CheckIfExistsByName(name);

            if (alreadyExists)
            {
                return 0;
            }

            var category = new Category
            {
                Name = name,
                Description = description,
            };

            await this.categoriesRepository.AddAsync(category);
            await this.categoriesRepository.SaveChangesAsync();

            return category.Id;
        }

        public async Task DeleteAsync(int id)
        {
            var category = this.categoriesRepository.All().FirstOrDefault(x => x.Id == id);

            if (category == null)
            {
                return;
            }

            var posts = this.postsRepository.All()
                .Where(x => x.CategoryId == id);

            foreach (var post in posts)
            {
                this.postsRepository.Delete(post);
            }

            await this.postsRepository.SaveChangesAsync();

            var categoryForums = this.forumCategoriesRepository.All()
                .Where(x => x.CategoryId == id).ToList();

            foreach (var categoryForum in categoryForums)
            {
                this.forumCategoriesRepository.Delete(categoryForum);
            }

            await this.forumCategoriesRepository.SaveChangesAsync();

            this.categoriesRepository.Delete(category);
            await this.categoriesRepository.SaveChangesAsync();
        }

        public string GetNormalisedName(string name)
        {
            var categories = this.categoriesRepository.All().Select(x => x.Name).ToList();

            var categoryName = categories.FirstOrDefault(x => UrlParser.ParseToUrl(x) == name);

            return categoryName;
        }

        private bool CheckIfExistsByName(string name)
        {
            bool alreadyExists = false;
            var category = this.categoriesRepository.All().FirstOrDefault(x => x.Name == name);

            if (category != null)
            {
                alreadyExists = true;
            }

            return alreadyExists;
        }
    }
}