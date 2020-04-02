using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GamersHub.Common;
using GamersHub.Data.Common.Repositories;
using GamersHub.Data.Models;
using GamersHub.Services.Mapping;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;

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

        public T GetById<T>(int id)
        {
            var forum = this.forumsRepository.All().Where(x => x.Id == id)
                .To<T>().FirstOrDefault();

            return forum;
        }

        public async Task AddForumCategoryIfCategoryDoesNotExist(int forumId, int categoryId)
        {
            var forum = this.forumsRepository
                .All()
                .FirstOrDefault(x => x.Id == forumId);

            if (forum == null)
            {
                return;
            }

            var forumCategoryExists = forum.ForumCategories
                .Select(fc => fc.CategoryId).Contains(categoryId);

            if (forumCategoryExists)
            {
                return;
            }

            var forumCategory = new ForumCategory
            {
                ForumId = forumId,
                CategoryId = categoryId,
            };

            forum.ForumCategories.Add(forumCategory);

            this.forumsRepository.Update(forum);
            await this.forumsRepository.SaveChangesAsync();
        }

        public async Task<int> CreateAsync(string name)
        {
            bool alreadyExists = this.CheckIfExistsByName(name);

            if (alreadyExists)
            {
                return 0;
            }

            var forum = new Forum
            {
                Name = name,
            };

            await this.forumsRepository.AddAsync(forum);
            await this.forumsRepository.SaveChangesAsync();

            return forum.Id;
        }

        public async Task DeleteAsync(int id)
        {
            var reply = this.forumsRepository.All()
                .FirstOrDefault(x => x.Id == id);

            this.forumsRepository.Delete(reply);
            await this.forumsRepository.SaveChangesAsync();
        }

        private bool CheckIfExistsByName(string name)
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