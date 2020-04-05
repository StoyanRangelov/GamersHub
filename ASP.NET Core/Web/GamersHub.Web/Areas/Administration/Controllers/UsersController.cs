using System.Linq;
using System.Threading.Tasks;
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
        private readonly UserManager<ApplicationUser> userManager;


        public UsersController(IUsersService usersService, UserManager<ApplicationUser> userManager)
        {
            this.usersService = usersService;
            this.userManager = userManager;
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
            var user = this.userManager.Users.FirstOrDefault(x => x.Id == input.UserId);

            await this.userManager.AddToRoleAsync(user, input.RoleName);
           
            return this.RedirectToAction("Index", "Dashboard");
        }
    }
}