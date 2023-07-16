using Microsoft.EntityFrameworkCore;

namespace MVC_News.Models
{
    public class NewsDbContext:DbContext
    {
        public NewsDbContext(DbContextOptions<NewsDbContext> options):base(options)
        {
            
        }

        public virtual DbSet<Category> Categories { get; set; }
        public virtual DbSet<NewsItem> NewsItems { get; set; }
        
    }
}
