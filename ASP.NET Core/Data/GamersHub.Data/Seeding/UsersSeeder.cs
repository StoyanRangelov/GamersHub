namespace GamersHub.Data.Seeding
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;

    using GamersHub.Data.Models;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.EntityFrameworkCore.Internal;
    using Microsoft.Extensions.DependencyInjection;

    public class UsersSeeder : ISeeder
    {
        public async Task SeedAsync(ApplicationDbContext dbContext, IServiceProvider serviceProvider)
        {
            var userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();

            if (dbContext.Users.Any())
            {
                return;
            }

            await userManager.CreateAsync(
                new ApplicationUser
            {
                Email = "admin@mail.com",
                UserName = "administrator",
                GamingExperience = GamingExperienceType.Advanced,
                DiscordUsername = "Admin#1234",
            }, "9j7hgdyw");

            await userManager.CreateAsync(
                new ApplicationUser
                {
                    Email = "mod@mail.com",
                    UserName = "moderator",
                    GamingExperience = GamingExperienceType.Advanced,
                    DiscordUsername = "Mod#1234",
                }, "9j7hgdyw");

            await userManager.CreateAsync(
                new ApplicationUser
                {
                    Email = "test2@mail.com",
                    UserName = "testUser1",
                    GamingExperience = GamingExperienceType.Advanced,
                    DiscordUsername = "Test1#1234",
                }, "9j7hgdyw");

            await userManager.CreateAsync(
                new ApplicationUser
                {
                    Email = "test1@mail.com",
                    UserName = "testUser2",
                    GamingExperience = GamingExperienceType.Advanced,
                    DiscordUsername = "Test2#1234",
                }, "9j7hgdyw");
        }
    }
}
