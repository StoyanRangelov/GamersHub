using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GamersHub.Data.Common.Repositories;
using GamersHub.Data.Models;
using GamersHub.Services.Mapping;

namespace GamersHub.Services.Data
{
    public class ForumsService : IForumsService
    {
        private readonly IDeletableEntityRepository<Forum> forumsRepository;

        public ForumsService(IDeletableEntityRepository<Forum> forumsRepository)
        {
            this.forumsRepository = forumsRepository;
        }

        public IEnumerable<T> GetAll<T>(int? count = null)
        {
            IQueryable<Forum> query =
                this.forumsRepository.All()
                    .OrderByDescending(x => x.Posts.Count)
                    .ThenByDescending(x => x.ForumCategories.Count);
            if (count.HasValue)
            {
                query = query.Take(count.Value);
            }

            return query.To<T>().ToList();
        }

        public T GetByName<T>(string name)
        {
            string originalName = name.Replace('-', ' ');

            var forum = this.forumsRepository.All().Where(x => x.Name == originalName)
                .To<T>().FirstOrDefault();
            return forum;
        }

        public T GetById<T>(int id)
        {
            var forum = this.forumsRepository.All()
                .Where(x => x.Id == id).To<T>().FirstOrDefault();

            return forum;
        }

        public async Task CreateAsync(string name)
        {
            await this.forumsRepository.AddAsync(new Forum
            {
                Name = name,
            });

            await this.forumsRepository.SaveChangesAsync();
        }
    }
}
