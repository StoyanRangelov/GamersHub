using GamersHub.Services.Data.Games;
using GamersHub.Web.ViewModels.Games;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GamersHub.Web.Controllers
{
    [Authorize]
    public class GamesController : BaseController
    {
        private readonly IGamesService gamesService;

        public GamesController(IGamesService gamesService)
        {
            this.gamesService = gamesService;
        }

        public IActionResult Index()
        {
            var games = this.gamesService.GetAll<GameViewModel>();

            var viewModel = new GameIndexViewModel{ Games = games };

            return this.View(viewModel);
        }

        public IActionResult ById(int id)
        {
            var viewModel = this.gamesService.GetById<GameByIdViewModel>(id);

            if (viewModel == null)
            {
                return this.NotFound();
            }

            return this.View(viewModel);
        }
    }
}