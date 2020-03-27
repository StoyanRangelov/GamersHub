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
                this.forumsRepository.All();
            if (count.HasValue)
            {
                query = query.Take(count.Value);
            }

            return query.To<T>().ToList();
        }

        public T GetByUrl<T>(string url)
        {
            var forums = this.forumsRepository.All().Select(x => x.Name).ToList();

            var forumToReturn = forums.FirstOrDefault(x => UrlParser.ParseToUrl(x) == url);

            var forum = this.forumsRepository.All().Where(x => x.Name == forumToReturn)
                .To<T>().FirstOrDefault();

            return forum;
        }

        public async Task<int> CreateAsync(string name)
        {
            var forum = new Forum
            {
                Name = name,
            };

            await this.forumsRepository.AddAsync(forum);
            await this.forumsRepository.SaveChangesAsync();

            return forum.Id;
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