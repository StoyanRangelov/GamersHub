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

        public IEnumerable<string> GetAllNames()
        {
            return this.forumsRepository.All().Select(x => x.Name).ToList();
        }

        public T GetByName<T>(string name)
        {
            string originalName = name.Replace('-', ' ');

            var forum = this.forumsRepository.All().Where(x => x.Name == originalName)
                .To<T>().FirstOrDefault();
            return forum;
        }

        public int GetIdByName(string name)
        {
            return this.forumsRepository.All().First(x => x.Name == name).Id;
        }

        public async Task CreateAsync(string name)
        {
            await this.forumsRepository.AddAsync(new Forum
            {
                Name = name,
            });

            await this.forumsRepository.SaveChangesAsync();
        }

        public bool CheckIfExistsByName(string name)
        {
            bool alreadyExists = false;

            var forum = this.forumsRepository.All().FirstOrDefault(x => x.Name == name);

            if (forum != null)
            {
                alreadyExists = true;
            }

            return alreadyExists;
        }
    }
}
