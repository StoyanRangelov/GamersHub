using System.Collections.Generic;

namespace GamersHub.Web.ViewModels.Parties
{
    public class PartyIndexViewModel
    {
        public int PagesCount { get; set; }

        public int CurrentPage { get; set; }

        public IEnumerable<PartyViewModel> Parties { get; set; }
    }
}