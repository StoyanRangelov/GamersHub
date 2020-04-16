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

namespace GamersHub.Services.Data.Categories
{
    public class CategoriesService : ICategoriesService
    {
        private readonly IDeletableEntityRepository<Category> categoriesRepository;
        private readonly IRepository<ForumCategory> forumCategoriesRepository;
        private readonly IDeletableEntityRepository<Post> postRepository;
        private readonly IDeletableEntityRepository<Reply> repliesRepository;

        public CategoriesService(
            IDeletableEntityRepository<Category> categoriesRepository,
            IRepository<ForumCategory> forumCategoriesRepository,
            IDeletableEntityRepository<Post> postRepository,
            IDeletableEntityRepository<Reply> repliesRepository)
        {
            this.categoriesRepository = categoriesRepository;
            this.forumCategoriesRepository = forumCategoriesRepository;
            this.postRepository = postRepository;
            this.repliesRepository = repliesRepository;
        }

        public IEnumerable<T> GetAll<T>(int? take = null, int skip = 0)
        {
            IQueryable<Category> query =
                this.categoriesRepository.All()
                    .OrderByDescending(x => x.Posts.Count)
                    .ThenByDescending(x => x.CategoryForums.Count).Skip(skip);
            if (take.HasValue)
            {
                query = query.Take(take.Value);
            }

            return query.To<T>().ToList();
        }

        public IEnumerable<T> GetAllMissingByForumId<T>(int id)
        {
            var categories = this.categoriesRepository.All()
                .Where(x => x.CategoryForums.Select(x => x.ForumId).All(x => !x.Equals(id)))
                .To<T>().ToList();

            return categories;
        }

        public T GetById<T>(int id)
        {
            var category = this.categoriesRepository.All()
                .Where(x => x.Id == id)
                .To<T>().FirstOrDefault();

            return category;
        }

        public async Task<int> CreateAsync(string name, string description)
        {
            var alreadyExists = this.CheckIfExistsByName(name);

            if (alreadyExists)
            {
                return 0;
            }

            var category = new Category
            {
                Name = name,
                Description = description,
            };

            await this.categoriesRepository.AddAsync(category);
            await this.categoriesRepository.SaveChangesAsync();

            return category.Id;
        }

        public async Task<int> EditAsync(int id, string name, string description, int[] forumIds, bool[] areSelected)
        {
            var category = this.categoriesRepository.All()
                .FirstOrDefault(x => x.Id == id);

            if (category == null)
            {
                return -1;
            }

            if (category.Name != name)
            {
                var alreadyExists = this.CheckIfExistsByName(name);

                if (alreadyExists)
                {
                    return 0;
                }
            }

            category.Name = name;
            category.Description = description;

            if (forumIds != null)
            {
                for (int i = 0; i < forumIds.Length; i++)
                {
                    if (areSelected[i])
                    {
                        var categoryForum = new ForumCategory
                        {
                            CategoryId = id,
                            ForumId = forumIds[i],
                        };

                        category.CategoryForums.Add(categoryForum);
                    }
                }
            }

            this.categoriesRepository.Update(category);
            await this.categoriesRepository.SaveChangesAsync();

            return category.Id;
        }

        public async Task DeleteAsync(int id)
        {
            var category = this.categoriesRepository.All()
                .Include(x => x.CategoryForums)
                .Include(x => x.Posts)
                .ThenInclude(x => x.Replies)
                .FirstOrDefault(x => x.Id == id);

            if (category == null)
            {
                return;
            }

            foreach (var post in category.Posts)
            {
                 this.postRepository.Delete(post);

                 foreach (var reply in post.Replies)
                 {
                     this.repliesRepository.Delete(reply);
                 }
            }

            foreach (var categoryForum in category.CategoryForums)
            {
                this.forumCategoriesRepository.Delete(categoryForum);
            }

            await this.postRepository.SaveChangesAsync();
            await this.repliesRepository.SaveChangesAsync();
            await this.forumCategoriesRepository.SaveChangesAsync();

            this.categoriesRepository.Delete(category);
            await this.categoriesRepository.SaveChangesAsync();
        }

        public string GetNormalisedName(string name)
        {
            var categories = this.categoriesRepository.All().Select(x => x.Name).ToList();

            var categoryName = categories.FirstOrDefault(x => UrlParser.ParseToUrl(x) == name);

            return categoryName;
        }

        public int GetCount()
        {
            return this.categoriesRepository.All().Count();
        }


        /// <summary>
        /// Returns true if a category with the given name already exists in the database, otherwise returns false
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        private bool CheckIfExistsByName(string name)
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