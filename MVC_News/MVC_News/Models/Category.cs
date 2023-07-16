using System.ComponentModel.DataAnnotations;

namespace MVC_News.Models
{
    public class Category
    {
        public int Id { get; set; }
        [Required, StringLength(20, MinimumLength = 3)]
        public string Name { get; set; }
        public Category()
        {
            NewsItems = new HashSet<NewsItem>();
        }
        public virtual ICollection<NewsItem> NewsItems { get; set; }
    }
}
