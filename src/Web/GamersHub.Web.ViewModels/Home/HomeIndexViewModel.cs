namespace GamersHub.Web.ViewModels.Home
{
    using System.Collections.Generic;

    public class HomeIndexViewModel
    {
        public IEnumerable<GameHomeIndexViewModel> Games { get; set; }

        public IEnumerable<PostHomeIndexViewModel> Posts { get; set; }

        public IEnumerable<PartyHomeIndexViewModel> Parties { get; set; }

        public IEnumerable<UserHomeIndexViewModel> TopUsers { get; set; }
    }
}
