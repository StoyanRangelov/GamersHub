namespace GamersHub.Web.ViewModels.Home
{
    using System;

    using GamersHub.Data.Models;
    using GamersHub.Services.Mapping;

    public class PartyHomeIndexViewModel : IMapFrom<Party>
    {
        public string Game { get; set; }

        public string Activity { get; set; }

        public int Size { get; set; }

        public bool IsFull { get; set; }

        public string CreatorUsername { get; set; }

        public DateTime CreatedOn { get; set; }

        public string PartyApplicantsCount { get; set; }
    }
}
