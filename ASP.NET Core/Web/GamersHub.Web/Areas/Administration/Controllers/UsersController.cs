using System.Linq;
using System.Threading.Tasks;
using GamersHub.Common;
using GamersHub.Data.Models;
using GamersHub.Services.Data.Users;
using GamersHub.Web.ViewModels.Administration.Users;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace GamersHub.Web.Areas.Administration.Controllers
{
    public class UsersController : AdministrationController
    {
        private readonly IUsersService usersService;


        public UsersController(IUsersService usersService)
        {
            this.usersService = usersService;
        }

        public IActionResult Index()
        {
            var users = this.usersService.GetAllPromotableUsers<UserViewModel>();

            var viewModel = new UserIndexViewModel {Users = users};

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

        public IActionResult Banned()
        {
            var users = this.usersService.GetAllBannedUsers<UserBannedViewModel>();

            var viewModel = new UserAdministrationBannedViewModel {BannedUsers = users};

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