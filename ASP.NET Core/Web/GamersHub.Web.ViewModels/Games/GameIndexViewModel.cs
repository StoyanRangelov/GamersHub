using System.Collections.Generic;

namespace GamersHub.Web.ViewModels.Games
{
    public class GameIndexViewModel
    {
        public int PagesCount { get; set; }

        public int CurrentPage { get; set; }

        public IEnumerable<GameViewModel> Games { get; set; }
    }
}