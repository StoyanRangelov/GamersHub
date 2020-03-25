using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GamersHub.Common;
using GamersHub.Data.Common.Repositories;
using GamersHub.Data.Models;
using GamersHub.Services.Mapping;

namespace GamersHub.Services.Data
{
    public class CategoriesService : ICategoriesService
    {
        private readonly IDeletableEntityRepository<Category> categoriesRepository;

        public CategoriesService(IDeletableEntityRepository<Category> categoriesRepository)
        {
            this.categoriesRepository = categoriesRepository;
        }

        public string GetNameByUrl(string url)
        {
            var categoryNames = this.categoriesRepository.All().Select(x => x.Name).ToList();

            var categoryName = categoryNames.FirstOrDefault(x => UrlParser.ParseToUrl(x) == url);

            return categoryName;
        }

        public IEnumerable<T> GetAll<T>(int? count = null)
        {
            IQueryable<Category> query =
                this.categoriesRepository.All()
                    .OrderByDescending(x => x.Posts.Count)
                    .ThenByDescending(x => x.CategoryForums.Count);
            if (count.HasValue)
            {
                query = query.Take(count.Value);
            }

            return query.To<T>().ToList();
        }

        public IEnumerable<string> GetAllNames()
        {
            return this.categoriesRepository.All().Select(x => x.Name).ToList();
        }

        public async Task CreateAsync(string name, string description)
        {
            var category = new Category
            {
                Name = name,
                Description = description,
            };

            await this.categoriesRepository.AddAsync(category);
            await this.categoriesRepository.SaveChangesAsync();
        }

        public bool CheckIfExistsByName(string name)
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