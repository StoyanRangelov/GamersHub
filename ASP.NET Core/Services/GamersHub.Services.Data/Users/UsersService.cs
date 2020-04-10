using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GamersHub.Common;
using GamersHub.Data.Common.Repositories;
using GamersHub.Data.Models;
using GamersHub.Services.Data.Posts;
using GamersHub.Services.Data.Replies;
using GamersHub.Services.Data.Reviews;
using GamersHub.Services.Mapping;
using Microsoft.AspNetCore.Identity;

namespace GamersHub.Services.Data.Users
{
    public class UsersService : IUsersService
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly RoleManager<ApplicationRole> roleManager;
        private readonly IDeletableEntityRepository<ApplicationUser> usersRepository;

        public UsersService(
            UserManager<ApplicationUser> userManager,
            RoleManager<ApplicationRole> roleManager,
            IDeletableEntityRepository<ApplicationUser> usersRepository)
        {
            this.userManager = userManager;
            this.roleManager = roleManager;
            this.usersRepository = usersRepository;
        }

        public IEnumerable<T> GetAllPromotableUsers<T>()
        {
            var administrator = this.roleManager.Roles
                .First(x => x.Name == GlobalConstants.AdministratorRoleName);

            var moderator = this.roleManager.Roles
                .First(x => x.Name == GlobalConstants.ModeratorRoleName);


            var users = this.userManager.Users
                .Where(x => x.Roles
                    .Select(x => x.RoleId).All(x => !x.Equals(moderator.Id)))
                .Where(x => x.Roles
                    .Select(x => x.RoleId).All(x => !x.Equals(administrator.Id)))
                .Where(x => x.LockoutEnd == null)
                .OrderByDescending(x => x.CreatedOn)
                .To<T>().ToList();

            return users;
        }

        public IEnumerable<T> GetAllBannedUsers<T>()
        {
            var users = this.userManager.Users
                .Where(x => x.LockoutEnd != null)
                .OrderByDescending(x => x.LockoutEnd)
                .To<T>().ToList();

            return users;
        }

        public IEnumerable<T> GetAllAdministrators<T>()
        {
            var administrator = this.roleManager.Roles
                .First(x => x.Name == GlobalConstants.AdministratorRoleName);

            var administrators = this.userManager.Users
                .Where(x => x.Roles
                    .Select(x => x.RoleId).Any(x => x.Equals(administrator.Id)))
                .To<T>().ToList();

            return administrators;
        }

        public IEnumerable<T> GetAllModerators<T>()
        {
            var moderator = this.roleManager.Roles
                .First(x => x.Name == GlobalConstants.ModeratorRoleName);

            var moderators = this.userManager.Users
                .Where(x => x.Roles
                    .Select(x => x.RoleId).Any(x => x.Equals(moderator.Id)))
                .To<T>().ToList();

            return moderators;
        }

        public IEnumerable<T> GetTopFiveForumUsers<T>()
        {
            var users = this.userManager.Users
                .OrderByDescending(x => x.Posts.Count)
                .Take(5).To<T>().ToList();

            return users;
        }

        public IEnumerable<T> GetTopFiveGameUsers<T>()
        {
            var users = this.userManager.Users
                .OrderByDescending(x => x.Reviews.Count)
                .Take(5).To<T>().ToList();

            return users;
        }

        public IEnumerable<T> GetTopFiveBanned<T>()
        {
            var users = this.userManager.Users
                .Where(x => x.LockoutEnd != null)
                .OrderByDescending(x => x.LockoutEnd)
                .Take(5).To<T>().ToList();

            return users;
        }

        public T GetById<T>(string id)
        {
            var user = this.userManager.Users
                .Where(x => x.Id == id).To<T>().FirstOrDefault();

            return user;
        }

        public async Task PromoteAsync(string id, string role)
        {
            var user = this.userManager.Users.FirstOrDefault(x => x.Id == id);

            await this.userManager.AddToRoleAsync(user, role);
        }

        public async Task DemoteAsync(string id)
        {
            var user = this.userManager.Users.FirstOrDefault(x => x.Id == id);

            await this.userManager.RemoveFromRoleAsync(user, GlobalConstants.ModeratorRoleName);
        }

        public async Task BanAsync(string id)
        {
            var user = this.userManager.Users.FirstOrDefault(x => x.Id == id);

            var dateTimeOffset = new DateTimeOffset(DateTime.UtcNow);
            var banLength = dateTimeOffset.AddDays(30);

            await this.userManager.SetLockoutEndDateAsync(user, banLength);
        }

        public async Task UnbanAsync(string id)
        {
            var user = this.userManager.Users.FirstOrDefault(x => x.Id == id);

            await this.userManager.SetLockoutEndDateAsync(user, null);
        }

        public async Task<bool> DeleteAsync(string id)
        {
            var user = this.usersRepository.All().FirstOrDefault(x => x.Id == id);

            if (user == null)
            {
                return false;
            }

            var isAdministrator = await this.userManager.IsInRoleAsync(user, GlobalConstants.AdministratorRoleName);
            var isModerator = await this.userManager.IsInRoleAsync(user, GlobalConstants.ModeratorRoleName);


            if (isAdministrator)
            {
                await this.userManager.RemoveFromRoleAsync(user, GlobalConstants.AdministratorRoleName);
            }

            if (isModerator)
            {
                await this.userManager.RemoveFromRoleAsync(user, GlobalConstants.ModeratorRoleName);
            }

            user.UserName = null;
            user.NormalizedUserName = null;
            this.usersRepository.Delete(user);

            await this.usersRepository.SaveChangesAsync();

            return true;
        }
    }
}
