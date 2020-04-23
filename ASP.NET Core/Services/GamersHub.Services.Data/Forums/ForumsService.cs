namespace GamersHub.Services.Data.Forums
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using GamersHub.Common;
    using GamersHub.Data.Common.Repositories;
    using GamersHub.Data.Models;
    using GamersHub.Services.Mapping;
    using Microsoft.EntityFrameworkCore;

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

        public IEnumerable<T> GetAll<T>(int? take = null, int skip = 0)
        {
            IQueryable<Forum> query =
                this.forumsRepository.All()
                    .OrderByDescending(x => x.Posts.Count)
                    .ThenByDescending(x => x.ForumCategories.Count).Skip(skip);
            if (take.HasValue)
            {
                query = query.Take(take.Value);
            }

            return query.To<T>().ToList();
        }

        public IEnumerable<T> GetAllMissingByCategoryId<T>(int id)
        {
            var forums = this.forumsRepository.All()
                .Where(x => x.ForumCategories
                    .Select(x => x.CategoryId).All(x => !x.Equals(id)))
                .To<T>().ToList();

            return forums;
        }

        public T GetByNameUrl<T>(string name)
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

        public async Task<int?> EditAsync(int id, string name, int[] categoryIds, bool[] areSelected)
        {
            var forum = this.forumsRepository.All()
                .Include(x => x.Posts)
                .FirstOrDefault(x => x.Id == id);

            if (forum == null)
            {
                return null;
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

        public async Task<int?> DeleteAsync(int id)
        {
            var forum = this.forumsRepository.All()
                .Include(x => x.ForumCategories)
                .Include(x => x.Posts)
                .ThenInclude(x => x.Replies)
                .FirstOrDefault(x => x.Id == id);

            if (forum == null)
            {
                return null;
            }

            foreach (var post in forum.Posts)
            {
                foreach (var reply in post.Replies)
                {
                    this.repliesRepository.Delete(reply);
                }

                this.postsRepository.Delete(post);
            }

            await this.postsRepository.SaveChangesAsync();
            await this.repliesRepository.SaveChangesAsync();

            this.forumsRepository.Delete(forum);
            await this.forumsRepository.SaveChangesAsync();

            foreach (var forumCategory in forum.ForumCategories)
            {
                this.forumCategoriesRepository.Delete(forumCategory);
            }

            await this.forumCategoriesRepository.SaveChangesAsync();

            return forum.Id;
        }

        public int GetCount()
        {
            return this.forumsRepository.All().Count();
        }

        /// <summary>
        /// Returns the normalised version of the provided forum name after comparing it to all other forum names through the UrlParser
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        private string GetNormalisedName(string name)
        {
            var forums = this.forumsRepository.All().Select(x => x.Name).ToList();

            var forumName = forums.FirstOrDefault(x => UrlParser.ParseToUrl(x) == name);

            return forumName;
        }

        /// <summary>
        /// Returns true if a forum with the given name already exists in the database, otherwise returns false
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
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
