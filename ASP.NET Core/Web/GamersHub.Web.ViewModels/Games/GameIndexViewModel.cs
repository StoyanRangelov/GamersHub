using System.Collections.Generic;

namespace GamersHub.Web.ViewModels.Games
{
    public class GameIndexViewModel
    {
        public PaginationViewModel Pagination { get; set; }

        public IEnumerable<GameViewModel> Games { get; set; }
    }
}