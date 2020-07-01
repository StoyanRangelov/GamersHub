namespace GamersHub.Data.Seeding
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using GamersHub.Data.Models;

    public class CategoriesSeeder : ISeeder
    {
        public async Task SeedAsync(ApplicationDbContext dbContext, IServiceProvider serviceProvider)
        {
            if (dbContext.Categories.Any())
            {
                return;
            }

            var categories = new List<(string Name, string Description)>
            {
                ("Guides", "Share game guides or useful tips."),
                ("Lore", "Discuss Lore or share your role-playing experience."),
                ("PVP", "Discuss player versus player strategies."),
                ("Bugs", "Share any bugs you found while playing the game."),
                ("Gameplay", "Gameplay discussion."),
            };

            foreach (var category in categories)
            {
                await dbContext.Categories.AddAsync(new Category
                {
                    Name = category.Name,
                    Description = category.Description,
                });
            }
        }
    }
}
