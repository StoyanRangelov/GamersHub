using System.Collections.Generic;

namespace GamersHub.Web.ViewModels.Administration.Moderators
{
    public class ModeratorIndexViewModel
    {
        public IEnumerable<ModeratorViewModel> Moderators { get; set; }
    }
}