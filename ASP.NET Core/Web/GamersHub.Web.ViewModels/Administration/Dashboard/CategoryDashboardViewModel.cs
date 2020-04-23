namespace GamersHub.Web.ViewModels.Administration.Dashboard
{
    using GamersHub.Data.Models;
    using GamersHub.Services.Mapping;

    public class CategoryDashboardViewModel : IMapFrom<Category>
    {
        public string Name { get; set; }

        public int PostsCount { get; set; }
    }
}
