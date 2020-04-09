using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GamersHub.Data.Common.Repositories;
using GamersHub.Data.Models;
using GamersHub.Services.Data.ForumCategories;
using GamersHub.Services.Data.Posts;
using GamersHub.Services.Mapping;

namespace GamersHub.Services.Data.Forums
{
    public class ForumsService : IForumsService
    {
        private readonly IDeletableEntityRepository<Forum> forumsRepository;
        private readonly IDeletableEntityRepository<Post> postsRepository;
        private readonly IRepository<ForumCategory> forumCategoriesRepository;


        public ForumsService(
            IDeletableEntityRepository<Forum> forumsRepository,
            IDeletableEntityRepository<Post> postsRepository,
            IRepository<ForumCategory> forumCategoriesRepository)
        {
            this.forumsRepository = forumsRepository;
            this.postsRepository = postsRepository;
            this.forumCategoriesRepository = forumCategoriesRepository;
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
                .FirstOrDefault(x => x.Id == id);

            if (forum == null)
            {
                return;
            }

            var posts = this.postsRepository.All()
                .Where(x => x.ForumId == id).ToList();

            foreach (var post in posts)
            {
                this.postsRepository.Delete(post);
            }

            await this.postsRepository.SaveChangesAsync();

            var forumCategories = this.forumCategoriesRepository.All()
                .Where(x => x.ForumId == id).ToList();

            foreach (var forumCategory in forumCategories)
            {
                this.forumCategoriesRepository.Delete(forumCategory);
            }

            await this.forumCategoriesRepository.SaveChangesAsync();


            this.forumsRepository.Delete(forum);
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