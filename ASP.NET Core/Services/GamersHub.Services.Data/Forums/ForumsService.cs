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

namespace GamersHub.Services.Data.Forums
{
    public class ForumsService : IForumsService
    {
        private readonly IDeletableEntityRepository<Forum> forumsRepository;
        private readonly IDeletableEntityRepository<Post> postsRepository;
        private readonly IDeletableEntityRepository<Reply> repliesRepository;
        private readonly IRepository<ForumCategory> forumCategoriesRepository;


        public ForumsService(
            IDeletableEntityRepository<Forum> forumsRepository,
            IRepository<ForumCategory> forumCategoriesRepository,
            IDeletableEntityRepository<Post> postsRepository,
            IDeletableEntityRepository<Reply> repliesRepository)
        {
            this.forumsRepository = forumsRepository;
            this.forumCategoriesRepository = forumCategoriesRepository;
            this.postsRepository = postsRepository;
            this.repliesRepository = repliesRepository;
        }

        public IEnumerable<T> GetAll<T>(int? count = null, int skip = 0)
        {
            IQueryable<Forum> query =
                this.forumsRepository.All()
                    .OrderByDescending(x => x.Posts.Count).Skip(skip);
            if (count.HasValue)
            {
                query = query.Take(count.Value);
            }

            return query.To<T>().ToList();
        }

        public IEnumerable<T> GetTopFive<T>()
        {
            var forums = this.forumsRepository.All()
                .OrderByDescending(x => x.Posts.Count)
                .Take(5).To<T>().ToList();

            return forums;
        }

        public IEnumerable<T> GetAllByCategoryId<T>(int id)
        {
            var forums = this.forumsRepository.All()
                .Where(x => x.ForumCategories
                    .Select(x => x.CategoryId).All(x => !x.Equals(id)))
                .To<T>().ToList();

            return forums;
        }

        public T GetByName<T>(string name)
        {
            var normalisedName = this.GetNormalisedName(name);

            var forum = this.forumsRepository.All()
                .Where(x => x.Name == normalisedName)
                .To<T>().FirstOrDefault();

            return forum;
        }

        public T GetById<T>(int id)
        {
            var forum = this.forumsRepository.All().Where(x => x.Id == id)
                .To<T>().FirstOrDefault();

            return forum;
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

        public async Task<int> EditAsync(int id, string name, int[] categoryIds, bool[] areSelected)
        {
            var forum = this.forumsRepository.All()
                .Include(x => x.Posts)
                .FirstOrDefault(x => x.Id == id);

            if (forum == null)
            {
                return -1;
            }

            if (forum.Name != name)
            {
                var alreadyExists = this.CheckIfExistsByName(name);

                if (alreadyExists)
                {
                    return 0;
                }
            }

            forum.Name = name;

            if (categoryIds != null)
            {
                for (int i = 0; i < categoryIds.Length; i++)
                {
                    if (areSelected[i])
                    {
                        var forumCategory = new ForumCategory
                        {
                            ForumId = id,
                            CategoryId = categoryIds[i],
                        };

                        forum.ForumCategories.Add(forumCategory);
                    }
                }
            }

            this.forumsRepository.Update(forum);
            await this.forumsRepository.SaveChangesAsync();

            return forum.Id;
        }

        public async Task DeleteAsync(int id)
        {
            var forum = this.forumsRepository.All()
                .Include(x => x.ForumCategories)
                .Include(x => x.Posts)
                .ThenInclude(x => x.Replies)
                .FirstOrDefault(x => x.Id == id);

            if (forum == null)
            {
                return;
            }

            foreach (var post in forum.Posts)
            {
                this.postsRepository.Delete(post);

                foreach (var reply in post.Replies)
                {
                    this.repliesRepository.Delete(reply);
                }
            }

            foreach (var forumCategory in forum.ForumCategories)
            {
                this.forumCategoriesRepository.Delete(forumCategory);
            }

            await this.postsRepository.SaveChangesAsync();
            await this.repliesRepository.SaveChangesAsync();
            await this.forumCategoriesRepository.SaveChangesAsync();

            this.forumsRepository.Delete(forum);
            await this.forumsRepository.SaveChangesAsync();
        }

        public int GetCount()
        {
            return this.forumsRepository.All().Count();
        }

        public string GetNormalisedName(string name)
        {
            var forums = this.forumsRepository.All().Select(x => x.Name).ToList();

            var forumName = forums.FirstOrDefault(x => UrlParser.ParseToUrl(x) == name);

            return forumName;
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