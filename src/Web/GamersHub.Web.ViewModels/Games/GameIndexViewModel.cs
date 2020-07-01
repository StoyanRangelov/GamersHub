namespace GamersHub.Web.ViewModels.Games
{
    using System.Collections.Generic;

    public class GameIndexViewModel
    {
        public int PagesCount { get; set; }

        public int CurrentPage { get; set; }

        public IEnumerable<GameViewModel> Games { get; set; }
    }
}
