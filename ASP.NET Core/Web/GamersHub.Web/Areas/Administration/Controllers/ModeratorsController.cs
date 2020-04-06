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
            var moderators = this.usersService.GetAllModerators<ModeratorViewModel>();

            var viewModel = new ModeratorIndexViewModel {Moderators = moderators};

            return this.View(viewModel);
        }
    }
}