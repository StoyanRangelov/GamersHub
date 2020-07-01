namespace GamersHub.Data.Seeding
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using GamersHub.Data.Models;

    public class ForumsSeeder : ISeeder
    {
        public async Task SeedAsync(ApplicationDbContext dbContext, IServiceProvider serviceProvider)
        {
            if (dbContext.Forums.Any())
            {
                return;
            }

            var forums = new List<string>
            {
                "Dota 2",
                "League of Legends",
                "Counter Strike",
                "World of Warcraft",
                "Battlefield",
            };

            foreach (var forum in forums)
            {
                await dbContext.Forums.AddAsync(new Forum
                {
                    Name = forum,
                    ForumCategories = new List<ForumCategory>
                    {
                        new ForumCategory
                        {
                            CategoryId = dbContext.Categories.First().Id,
                        },
                    },
                });
            }
        }
    }
}
