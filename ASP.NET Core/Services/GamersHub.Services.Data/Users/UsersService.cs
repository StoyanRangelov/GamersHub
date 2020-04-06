﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GamersHub.Common;
using GamersHub.Data.Common.Repositories;
using GamersHub.Data.Models;
using GamersHub.Services.Mapping;
using Microsoft.AspNetCore.Identity;

namespace GamersHub.Services.Data.Users
{
    public class UsersService : IUsersService
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly RoleManager<ApplicationRole> roleManager;

        public UsersService(UserManager<ApplicationUser> userManager, RoleManager<ApplicationRole> roleManager)
        {
            this.userManager = userManager;
            this.roleManager = roleManager;
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
                .Where(x=>x.LockoutEnd == null)
                .OrderByDescending(x=>x.Posts.Count)
                .ThenByDescending(x=>x.Replies.Count)
                .To<T>().ToList();

            return users;
        }

        public IEnumerable<T> GetAllBannedUsers<T>()
        {
            var users = this.userManager.Users
                .Where(x => x.LockoutEnd != null)
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

        public IEnumerable<T> GetTopFive<T>()
        {
            var users = this.userManager.Users
                .OrderByDescending(x => x.Posts.Count)
                .Take(5).To<T>().ToList();

            return users;
        }

        public IEnumerable<T> GetTopFiveBanned<T>()
        {
            var users = this.userManager.Users
                .Where(x=>x.LockoutEnd != null)
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

        public async Task BanAsync(string id)
        {
            var user = this.userManager.Users.FirstOrDefault(x => x.Id == id);

            var dateTimeOffset = new DateTimeOffset(DateTime.UtcNow);
            var banLength = dateTimeOffset.AddDays(30);

            await this.userManager.SetLockoutEndDateAsync(user, banLength);
        }

        public async Task<string> UnbanAsync(string id)
        {
            string userRole = string.Empty;

            var user = this.userManager.Users.FirstOrDefault(x => x.Id == id);

            bool isModerator = await this.userManager.IsInRoleAsync(user, GlobalConstants.ModeratorRoleName);

            if (isModerator)
            {
                userRole = GlobalConstants.ModeratorRoleName;
            }

            await this.userManager.SetLockoutEndDateAsync(user, null);

            return userRole;
        }
    }
}