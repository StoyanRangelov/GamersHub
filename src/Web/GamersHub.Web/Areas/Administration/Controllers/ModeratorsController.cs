namespace GamersHub.Web.Areas.Administration.Controllers
{
    using System.Threading.Tasks;

    using GamersHub.Common;
    using GamersHub.Data.Models;
    using GamersHub.Services.Data.Users;
    using GamersHub.Web.ViewModels.Administration.Moderators;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;

    public class ModeratorsController : AdministrationController
    {
        private readonly IUsersService usersService;
        private readonly UserManager<ApplicationUser> userManager;

        public ModeratorsController(IUsersService usersService, UserManager<ApplicationUser> userManager)
        {
            this.usersService = usersService;
            this.userManager = userManager;
        }

        public IActionResult Index()
        {
            var moderators = this.usersService
                .GetAllByRole<ModeratorViewModel>(GlobalConstants.ModeratorRoleName);

            var viewModel = new ModeratorIndexViewModel { Moderators = moderators };

            return this.View(viewModel);
        }

        public IActionResult Demote(string id)
        {
            var viewModel = this.usersService.GetById<ModeratorDemoteViewModel>(id);

            return this.View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Demote(ModeratorDemoteViewModel input)
        {
            var user = await this.userManager.FindByIdAsync(input.Id);
            if (user == null)
            {
                return this.NotFound();
            }

            await this.userManager.RemoveFromRoleAsync(user, GlobalConstants.ModeratorRoleName);

            this.TempData["InfoMessage"] = "Moderator demoted successfully!";
            return this.RedirectToAction("Index", "Dashboard");
        }
    }
}
