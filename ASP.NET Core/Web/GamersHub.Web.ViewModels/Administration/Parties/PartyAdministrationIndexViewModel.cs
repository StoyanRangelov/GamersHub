namespace GamersHub.Web.ViewModels.Administration.Parties
{
    using System.Collections.Generic;

    public class PartyAdministrationIndexViewModel
    {
        public PaginationViewModel Pagination { get; set; }

        public IEnumerable<PartyAdministrationViewModel> Parties { get; set; }
    }
}
