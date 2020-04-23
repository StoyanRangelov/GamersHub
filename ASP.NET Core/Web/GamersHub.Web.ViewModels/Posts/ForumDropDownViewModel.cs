namespace GamersHub.Web.ViewModels.Posts
{
    using GamersHub.Data.Models;
    using GamersHub.Services.Mapping;

    public class ForumDropDownViewModel : IMapFrom<Forum>
    {
        public int Id { get; set; }

        public string Name { get; set; }
    }
}
