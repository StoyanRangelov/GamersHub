using System.Collections.Generic;

namespace GamersHub.Web.ViewModels.Administration.Parties
{
    public class PartyAdministrationIndexViewModel
    {
        public PaginationViewModel Pagination { get; set; }

        public IEnumerable<PartyAdministrationViewModel> Parties { get; set; }
    }
}