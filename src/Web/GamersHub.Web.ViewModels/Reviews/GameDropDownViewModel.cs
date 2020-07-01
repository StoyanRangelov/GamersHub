namespace GamersHub.Web.ViewModels.Reviews
{
    using GamersHub.Data.Models;
    using GamersHub.Services.Mapping;

    public class GameDropDownViewModel : IMapFrom<Game>
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public string SubTitle { get; set; }
    }
}
