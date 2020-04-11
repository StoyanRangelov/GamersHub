using System.Threading.Tasks;
using GamersHub.Common;
using GamersHub.Services.Data.Users;
using GamersHub.Web.ViewModels.Administration.Moderators;
using Microsoft.AspNetCore.Mvc;

namespace GamersHub.Web.Areas.Administration.Controllers
{
    public class ModeratorsController : AdministrationController
    {
        private readonly IUsersService usersService;

        public ModeratorsController(IUsersService usersService)
        {
            this.usersService = usersService;
        }

        public IActionResult Index()
        {
            var moderators = this.usersService
                .GetAllByRole<ModeratorViewModel>(GlobalConstants.ModeratorRoleName);

            var viewModel = new ModeratorIndexViewModel {Moderators = moderators};

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
            await this.usersService.DemoteAsync(input.Id);

            this.TempData["InfoMessage"] = "Moderator demoted successfully!";
            return this.RedirectToAction("Index", "Dashboard");
        }
    }
}