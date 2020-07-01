namespace GamersHub.Web.ViewModels.Administration.Forums
{
    using GamersHub.Data.Models;
    using GamersHub.Services.Mapping;

    public class CategoryEditViewModel : IMapFrom<Category>
    {
        public int Id { get; set; }

        public string Name { get; set; }
    }
}
