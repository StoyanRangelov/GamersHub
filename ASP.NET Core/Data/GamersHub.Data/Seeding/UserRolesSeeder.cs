using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace GamersHub.Data.Seeding
{
    public class UserRolesSeeder : ISeeder
    {
        public async Task SeedAsync(ApplicationDbContext dbContext, IServiceProvider serviceProvider)
        {
            if (dbContext.UserRoles.Any())
            {
                return;
            }

            var userRole = dbContext.Roles.First(r => r.Name == "User");

            var users = dbContext.Users.ToList();

            foreach (var user in users)
            {
                await dbContext.UserRoles.AddAsync(new IdentityUserRole<string>
                {
                    UserId = user.Id,
                    RoleId = userRole.Id,
                });
            }

            var firstUser = users[0];
            var adminRole = dbContext.Roles.First(r => r.Name == "Administrator");

            await dbContext.UserRoles.AddAsync(new IdentityUserRole<string>
            {
                RoleId = adminRole.Id,
                UserId = firstUser.Id,
            });
        }
    }
}