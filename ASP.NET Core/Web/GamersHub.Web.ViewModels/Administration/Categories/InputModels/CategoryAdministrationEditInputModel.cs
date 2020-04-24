namespace GamersHub.Web.ViewModels.Administration.Categories.InputModels
{
    public class CategoryAdministrationEditInputModel
    {
        public int Id { get; set; }

        public CategoryEditInputModel Category { get; set; }

        public int[] ForumIds { get; set; }

        public bool[] AreSelected { get; set; }

        public ForumEditViewModel[] Forums { get; set; }
    }
}
