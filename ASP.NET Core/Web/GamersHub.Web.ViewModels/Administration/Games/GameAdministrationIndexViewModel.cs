using System.Collections.Generic;

namespace GamersHub.Web.ViewModels.Administration.Games
{
    public class GameAdministrationIndexViewModel
    {
        public IEnumerable<GameAdministrationViewModel> Games { get; set; }
    }
}