using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace SecurityCapstone.Data
{

    //    -- database.sql
    //CREATE TABLE Users(
    //UserID INT PRIMARY KEY AUTO_INCREMENT,
    //Username VARCHAR(100),
    //    Email VARCHAR(100)
    //);

    public class User
    {
        public int UserID { get; set; }
        public string Username { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;

        public override string ToString()
        {
            return $"UserID: {UserID}, Username: {Username}, Email: {Email}";
        }
    }

    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }

        public DbSet<User> Users => Set<User>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<User>().HasData(
                new User { UserID = 1, Username = "alice", Email = "alice@example.com" },
                new User { UserID = 2, Username = "bob", Email = "bob@example.com" },
                new User { UserID = 3, Username = "charlie", Email = "charlie@example.com" }
            );
        }
    }
}
