namespace GamersHub.Web.ViewModels.Parties
{
    using System.Collections.Generic;

    public class PartyIndexViewModel
    {
        public int PagesCount { get; set; }

        public int CurrentPage { get; set; }

        public IEnumerable<PartyViewModel> Parties { get; set; }
    }
}
