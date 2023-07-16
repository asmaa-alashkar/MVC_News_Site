using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace MVC_News.Models
{
    public class NewsItem
    {
        public int Id { get; set; }
        [Required, StringLength(50, MinimumLength = 3)]
        public string Title { get; set; }
        [Required, StringLength(500, MinimumLength = 5)]
        public string Description { get; set; }
        public DateTime CreatedDate { get; set; }
        public string? ImgSrc { get; set; }

        [ForeignKey(nameof(Category))]
        public int CategoryId { get; set; }
        public virtual Category? Category { get; set; }
    }
}
