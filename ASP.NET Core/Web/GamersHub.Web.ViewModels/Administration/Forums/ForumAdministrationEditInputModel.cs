namespace GamersHub.Web.ViewModels.Administration.Forums
{
    public class ForumAdministrationEditInputModel
    {
        public int Id { get; set; }

        public ForumEditInputModel Forum { get; set; }

        public int[] CategoryIds { get; set; }

        public bool[] AreSelected { get; set; }

        public CategoryEditViewModel[] Categories { get; set; }
    }
}
