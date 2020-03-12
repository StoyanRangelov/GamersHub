namespace GamersHub.Data.Models
{
   public class ForumCategory
    {
        public int ForumId { get; set; }

        public virtual Forum Forum { get; set; }

        public int CategoryId { get; set; }

        public virtual Category Category { get; set; }
    }
}
