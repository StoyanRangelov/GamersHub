﻿namespace GamersHub.Services.Data.Users
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using GamersHub.Common;
    using GamersHub.Data.Common.Repositories;
    using GamersHub.Data.Models;
    using GamersHub.Services.Mapping;

    public class UsersService : IUsersService
    {
        private readonly IDeletableEntityRepository<ApplicationUser> usersRepository;
        private readonly IDeletableEntityRepository<ApplicationRole> rolesRepository;

        public UsersService(
            IDeletableEntityRepository<ApplicationUser> usersRepository,
            IDeletableEntityRepository<ApplicationRole> rolesRepository)
        {
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

        public IEnumerable<T> GetTopFive<T>(string orderType = null)
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
                query = query.OrderByDescending(x => x.Posts.Count)
                    .ThenByDescending(x => x.Replies.Count)
                    .ThenByDescending(x => x.Reviews.Count)
                    .ThenByDescending(x => x.Parties.Count)
                    .ThenByDescending(x => x.CreatedOn);
            }

            return query.Take(5).To<T>().ToList();
        }

        public T GetById<T>(string id)
        {
            var user = this.usersRepository.All()
                .Where(x => x.Id == id).To<T>().FirstOrDefault();

            return user;
        }

        public T GetByName<T>(string name)
        {
            var user = this.usersRepository.All()
                .Where(x => x.UserName == name).To<T>().FirstOrDefault();

            return user;
        }

        public async Task<string> EditProfileAsync(string id, string discordUsername, GamingExperienceType gamingExperience, string imageUrl)
        {
            var user = this.usersRepository.All().FirstOrDefault(x => x.Id == id);
            if (user == null)
            {
                return null;
            }

            user.DiscordUsername = discordUsername;
            user.GamingExperience = gamingExperience;
            if (imageUrl != string.Empty)
            {
                user.ImgUrl = imageUrl;
            }

            this.usersRepository.Update(user);
            await this.usersRepository.SaveChangesAsync();

            return user.Id;
        }

        public async Task DeleteAsync(string id)
        {
            var user = this.usersRepository.All().First(x => x.Id == id);

            user.NormalizedUserName = null;
            user.Email = null;
            user.NormalizedEmail = null;
            user.DiscordUsername = string.Empty;
            user.ImgUrl = string.Empty;

            this.usersRepository.Update(user);
            this.usersRepository.Delete(user);

            await this.usersRepository.SaveChangesAsync();
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

        public string GetIdByName(string name)
        {
            return this.usersRepository.All().FirstOrDefault(x => x.UserName == name)?.Id;
        }
    }
}
