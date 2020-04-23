namespace GamersHub.Data.Seeding
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;

    using GamersHub.Common;
    using GamersHub.Data.Models;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.Extensions.DependencyInjection;

    public class UserRolesSeeder : ISeeder
    {
        public async Task SeedAsync(ApplicationDbContext dbContext, IServiceProvider serviceProvider)
        {
            var userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();

            if (dbContext.UserRoles.Any())
            {
                return;
            }

            var administrator = dbContext.Users.First(x => x.UserName == "administrator");
            var moderator = dbContext.Users.First(x => x.UserName == "moderator");

            await userManager.AddToRoleAsync(administrator, GlobalConstants.AdministratorRoleName);
            await userManager.AddToRoleAsync(moderator, GlobalConstants.ModeratorRoleName);
        }
    }
}
