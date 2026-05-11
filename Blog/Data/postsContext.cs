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
            var dbProvider = Configuration["DatabaseConfig:Provider"];

            if (dbProvider == "PostgreSQL")
            {
                var connectionString = Configuration.GetConnectionString("PostgresConnection");
                optionsBuilder.UseNpgsql(connectionString);
            }
            else
            {
                var connectionString = Configuration.GetConnectionString("SqliteConnection");
                optionsBuilder.UseSqlite(connectionString);
            }
        }

    }
}