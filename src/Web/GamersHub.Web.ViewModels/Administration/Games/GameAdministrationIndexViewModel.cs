namespace GamersHub.Web.ViewModels.Administration.Games
{
    using System.Collections.Generic;

    public class GameAdministrationIndexViewModel
    {
        public PaginationViewModel Pagination { get; set; }

        public IEnumerable<GameAdministrationViewModel> Games { get; set; }
    }
}
