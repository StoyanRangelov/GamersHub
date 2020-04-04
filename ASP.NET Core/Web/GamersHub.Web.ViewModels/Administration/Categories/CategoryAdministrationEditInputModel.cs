namespace GamersHub.Web.ViewModels.Administration.Categories
{
    public class CategoryAdministrationEditInputModel
    {
        public int Id { get; set; }

        public CategoryEditInputModel Category { get; set; }

        public ForumEditInputModel[] ForumsInput { get; set; }

        public ForumEditViewModel[] Forums { get; set; }
    }
}