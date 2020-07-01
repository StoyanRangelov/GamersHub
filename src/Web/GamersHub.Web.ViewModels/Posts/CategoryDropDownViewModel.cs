namespace GamersHub.Web.ViewModels.Posts
{
    using GamersHub.Data.Models;
    using GamersHub.Services.Mapping;

    public class CategoryDropDownViewModel : IMapFrom<Category>
    {
        public int Id { get; set; }

        public string Name { get; set; }
    }
}
