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
        private readonly IDeletableEntityRepository<ApplicationUser> usersRepository;
        private readonly IDeletableEntityRepository<ApplicationRole> rolesRepository;

        public UsersService(
            UserManager<ApplicationUser> userManager,
            IDeletableEntityRepository<ApplicationUser> usersRepository,
            IDeletableEntityRepository<ApplicationRole> rolesRepository)
        {
            this.userManager = userManager;
            this.usersRepository = usersRepository;
            this.rolesRepository = rolesRepository;
        }

        public IEnumerable<T> GetAllPromotableUsers<T>(int? take = null, int skip = 0)
        {
            var administratorRoleId = this.rolesRepository.All()
                .First(x => x.Name == GlobalConstants.AdministratorRoleName).Id;

            var moderatorRoleId = this.rolesRepository.All()
                .First(x => x.Name == GlobalConstants.ModeratorRoleName).Id;


            var query = this.usersRepository.All()
                .Where(x => x.Roles
                    .Select(x => x.RoleId).All(x => !x.Equals(moderatorRoleId)))
                .Where(x => x.Roles
                    .Select(x => x.RoleId).All(x => !x.Equals(administratorRoleId)))
                .Where(x => x.LockoutEnd == null)
                .OrderByDescending(x => x.CreatedOn).Skip(skip);
            if (take.HasValue)
            {
                query = query.Take(take.Value);
            }

            return query.To<T>().ToList();
            ;
        }

        public IEnumerable<T> GetAllBannedUsers<T>(int? take = null, int skip = 0)
        {
            var query = this.usersRepository.All()
                .Where(x => x.LockoutEnd != null)
                .OrderByDescending(x => x.LockoutEnd).Skip(skip);

            if (take.HasValue)
            {
                query = query.Take(take.Value);
            }


            return query.To<T>().ToList();
            ;
        }

        public IEnumerable<T> GetAllByRole<T>(string role)
        {
            var roleId = this.rolesRepository.All()
                .First(x => x.Name == role).Id;

            var usersInRole = this.usersRepository.All()
                .Where(x => x.Roles
                    .Select(x => x.RoleId).Any(x => x.Equals(roleId)))
                .To<T>().ToList();

            return usersInRole;
        }

        public IEnumerable<T> GetTopFive<T>(string orderType)
        {
            var query = this.usersRepository.All();

            if (orderType == GlobalConstants.Posts)
            {
                query = query.OrderByDescending(x => x.Posts.Count);
            }
            else if (orderType == GlobalConstants.Reviews)
            {
                query = query.OrderByDescending(x => x.Reviews.Count);
            }
            else if (orderType == GlobalConstants.Banned)
            {
                query = query.Where(x => x.LockoutEnd != null)
                    .OrderByDescending(x => x.LockoutEnd);
            }
            else
            {
                query = query.OrderByDescending(x => x.CreatedOn);
            }

            return query.Take(5).To<T>().ToList();
        }

        public T GetById<T>(string id)
        {
            var user = this.usersRepository.All()
                .Where(x => x.Id == id).To<T>().FirstOrDefault();

            return user;
        }

        public async Task PromoteAsync(string id, string role)
        {
            var user = this.usersRepository.All().FirstOrDefault(x => x.Id == id);

            await this.userManager.AddToRoleAsync(user, role);
        }

        public async Task DemoteAsync(string id)
        {
            var user = this.usersRepository.All().FirstOrDefault(x => x.Id == id);

            await this.userManager.RemoveFromRoleAsync(user, GlobalConstants.ModeratorRoleName);
        }

        public async Task BanAsync(string id)
        {
            var user = this.usersRepository.All().FirstOrDefault(x => x.Id == id);

            var dateTimeOffset = new DateTimeOffset(DateTime.UtcNow);
            var banLength = dateTimeOffset.AddDays(30);

            await this.userManager.SetLockoutEndDateAsync(user, banLength);
        }

        public async Task UnbanAsync(string id)
        {
            var user = this.usersRepository.All().FirstOrDefault(x => x.Id == id);

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

        public int GetCountOfPromotableUsers()
        {
            var administratorRoleId = this.rolesRepository.All()
                .First(x => x.Name == GlobalConstants.AdministratorRoleName).Id;

            var moderatorRoleId = this.rolesRepository.All()
                .First(x => x.Name == GlobalConstants.ModeratorRoleName).Id;

            return this.usersRepository.All().Count(x =>
                x.Roles.Select(x => x.RoleId).All(x => !x.Equals(administratorRoleId)) &&
                x.Roles.Select(x => x.RoleId).All(x => !x.Equals(moderatorRoleId)));
        }

        public int GetCountOfBannedUsers()
        {
            return this.usersRepository.All().Count(x => x.LockoutEnd != null);
        }
    }
}