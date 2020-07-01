namespace GamersHub.Web.ViewModels.Administration.Forums
{
    using GamersHub.Data.Models;
    using GamersHub.Services.Mapping;

    public class CategoryInForumViewModel : IMapFrom<ForumCategory>
    {
        public string CategoryName { get; set; }
    }
}
