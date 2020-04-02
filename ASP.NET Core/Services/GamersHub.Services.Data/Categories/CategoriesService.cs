using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GamersHub.Common;
using GamersHub.Data.Common.Repositories;
using GamersHub.Data.Models;
using GamersHub.Services.Data.Posts;
using GamersHub.Services.Mapping;
using Microsoft.EntityFrameworkCore;

namespace GamersHub.Services.Data.Categories
{
    public class CategoriesService : ICategoriesService
    {
        private readonly IDeletableEntityRepository<Category> categoriesRepository;
        private readonly IPostsService postsService;

        public CategoriesService(IDeletableEntityRepository<Category> categoriesRepository, IPostsService postsService)
        {
            this.categoriesRepository = categoriesRepository;
            this.postsService = postsService;
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
            var category = this.categoriesRepository.All()
                .Include(x=>x.Posts)
                .FirstOrDefault(x => x.Id == id);

            if (category == null)
            {
                return;
            }

            foreach (var categoryPost in category.Posts)
            {
                await this.postsService.DeleteAsync(categoryPost.Id);
            }

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