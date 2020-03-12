using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GamersHub.Data.Models;

namespace GamersHub.Data.Seeding
{
   public class ForumCategoriesSeeder : ISeeder
    {
        public async Task SeedAsync(ApplicationDbContext dbContext, IServiceProvider serviceProvider)
        {
            if (dbContext.ForumCategories.Any())
            {
                return;
            }

            var forumCategories = new List<(int ForumId, int CategoryId)>
            {
                (1, 1), (1, 5), (1, 3),
                (2, 1), (2, 2), (2, 3), (2, 4), (2, 5),
                (3, 1), (3, 4),
                (4, 5),
                (5, 3), (5, 4),
            };

            foreach (var forumCategory in forumCategories)
            {
                await dbContext.ForumCategories.AddAsync(new ForumCategory
                {
                    ForumId = forumCategory.ForumId,
                    Forum = dbContext.Forums.Find(forumCategory.ForumId),
                    CategoryId = forumCategory.CategoryId,
                    Category = dbContext.Categories.Find(forumCategory.CategoryId),
                });

            }
        }
    }
}
