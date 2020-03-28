using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GamersHub.Common;
using GamersHub.Data.Common.Repositories;
using GamersHub.Data.Models;
using GamersHub.Services.Mapping;
using GamersHub.Web.ViewModels.Categories;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;

namespace GamersHub.Services.Data
{
    public class CategoriesService : ICategoriesService
    {
        private readonly IDeletableEntityRepository<Category> categoriesRepository;

        private readonly IRepository<ForumCategory> forumCategoriesRepository;

        public CategoriesService(
            IDeletableEntityRepository<Category> categoriesRepository,
            IRepository<ForumCategory> forumCategoriesRepository)
        {
            this.categoriesRepository = categoriesRepository;
            this.forumCategoriesRepository = forumCategoriesRepository;
        }

        public CategoryByNameViewModel GetByNameAndForumId(string name, int id)
        {
            var categoryName = this.GetNormalisedName(name);

            var forumCategory = this.forumCategoriesRepository.All()
                .Where(x => x.Category.Name == categoryName && x.ForumId == id)
                .Select(x => new CategoryByNameViewModel
                {
                    ForumName = x.Forum.Name,
                    CategoryName = x.Category.Name,
                    CategoryPosts = x.Category.Posts
                        .Where(cp => cp.Forum.Id == id).Select(p => new PostInCategoryViewModel
                    {
                        Id = p.Id,
                        Name = p.Name,
                        CreatedOn = p.CreatedOn,
                        CategoryName = p.Category.Name,
                        UserUsername = p.User.UserName,
                        RepliesCount = p.Replies.Count,
                    }).ToList(),
                }).FirstOrDefault();

            return forumCategory;
        }

        public IEnumerable<T> GetAll<T>(int? count = null)
        {
            IQueryable<Category> query =
                this.categoriesRepository.All();
            if (count.HasValue)
            {
                query = query.Take(count.Value);
            }

            return query.To<T>().ToList();
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

        private string GetNormalisedName(string name)
        {
            var categories = this.categoriesRepository.All().Select(x => x.Name).ToList();

            var categoryName = categories.FirstOrDefault(x => UrlParser.ParseToUrl(x) == name);

            return categoryName;
        }
    }
}