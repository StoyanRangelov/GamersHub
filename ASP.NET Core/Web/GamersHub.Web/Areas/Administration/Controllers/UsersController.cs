﻿using System;
using System.Linq;
using System.Threading.Tasks;
using GamersHub.Common;
using GamersHub.Data.Models;
using GamersHub.Services.Data.Users;
using GamersHub.Web.ViewModels;
using GamersHub.Web.ViewModels.Administration.Users;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace GamersHub.Web.Areas.Administration.Controllers
{
    public class UsersController : AdministrationController
    {
        private const int UsersPerPage = 14;
        private const int BannedUsersPerPage = 14;

        private readonly IUsersService usersService;


        public UsersController(IUsersService usersService)
        {
            this.usersService = usersService;
        }

        public IActionResult Index(int id = 1)
        {
            var users = this.usersService
                .GetAllPromotableUsers<UserViewModel>(UsersPerPage, (id - 1) * UsersPerPage);

            var viewModel = new UserIndexViewModel {Users = users};

            var count = this.usersService.GetCountOfPromotableUsers();

            var pagination = new PaginationViewModel();

            pagination.PagesCount = (int) Math.Ceiling((double) count / UsersPerPage);
            if (pagination.PagesCount == 0)
            {
                pagination.PagesCount = 1;
            }

            pagination.CurrentPage = id;

            viewModel.Pagination = pagination;

            return this.View(viewModel);
        }

        public IActionResult Promote(string id)
        {
            var user = this.usersService.GetById<UserPromoteViewModel>(id);

            if (user == null)
            {
                return this.NotFound();
            }

            var viewModel = new UserAdministrationPromoteInputModel {User = user};

            return this.View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Promote(UserAdministrationPromoteInputModel input)
        {
            await this.usersService.PromoteAsync(input.UserId, input.RoleName);

            this.TempData["InfoMessage"] = "User promoted successfully!";
            return this.RedirectToAction("Index", "Dashboard");
        }

        public IActionResult Ban(string id)
        {
            var viewModel = this.usersService.GetById<UserBanViewModel>(id);

            return this.View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Ban(UserBanViewModel input)
        {
            await this.usersService.BanAsync(input.Id);

            this.TempData["InfoMessage"] = "User banned successfully!";
            return this.RedirectToAction(nameof(this.Banned));
        }

        public IActionResult Banned(int id = 1)
        {
            var users = this.usersService
                .GetAllBannedUsers<UserBannedViewModel>(BannedUsersPerPage, (id - 1) * BannedUsersPerPage);

            var viewModel = new UserAdministrationBannedViewModel {BannedUsers = users};

            var count = this.usersService.GetCountOfBannedUsers();

            var pagination = new PaginationViewModel();

            pagination.PagesCount = (int) Math.Ceiling((double) count / BannedUsersPerPage);
            if (pagination.PagesCount == 0)
            {
                pagination.PagesCount = 1;
            }

            pagination.CurrentPage = id;

            viewModel.Pagination = pagination;

            return this.View(viewModel);
        }

        public IActionResult Unban(string id)
        {
            var viewModel = this.usersService.GetById<UserUnbanViewModel>(id);

            return this.View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Unban(UserUnbanViewModel input)
        {
            await this.usersService.UnbanAsync(input.Id);

            this.TempData["InfoMessage"] = "User unbanned successfully!";
            return this.RedirectToAction(nameof(this.Banned));
        }
    }
}