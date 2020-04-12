using System.Collections.Generic;

namespace GamersHub.Web.ViewModels.Parties
{
    public class PartyIndexViewModel
    {
        public PaginationViewModel Pagination { get; set; }

        public IEnumerable<PartyViewModel> Parties { get; set; }
    }
}