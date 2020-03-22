using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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

        public int GetIdByName(string name)
        {
            return this.categoriesRepository.All().First(x => x.Name == name).Id;
        }

        public async Task CreateAsync(string name, string description)
        {
            await this.categoriesRepository.AddAsync(new Category
            {
                Name = name,
                Description = description,
            });

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