using System.Collections.Generic;
using System.Linq;
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

        public void Create(string name, string description)
        {
            this.categoriesRepository.AddAsync(new Category
            {
                Name = name,
                Description = description,
            }).GetAwaiter().GetResult();

            this.categoriesRepository.SaveChangesAsync().GetAwaiter().GetResult();
        }
    }
}
