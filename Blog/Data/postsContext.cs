using Microsoft.EntityFrameworkCore;

namespace Blog.Data
{
    public class PostsContext : DbContext
    {
        public DbSet<Post> Posts { get; set; }
        public DbSet<User> Users { get; set; }
        protected readonly IConfiguration Configuration;
        public PostsContext(IConfiguration configuration)
        {
            Configuration = configuration;
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var connectionString = Configuration.GetConnectionString("postsDb");
            if (string.IsNullOrWhiteSpace(connectionString))
            {
                throw new InvalidOperationException("The 'ConnectionStrings:postsDb' configuration value is missing or empty.");
            }

            optionsBuilder.UseSqlite(connectionString);
        }
        
    }
}