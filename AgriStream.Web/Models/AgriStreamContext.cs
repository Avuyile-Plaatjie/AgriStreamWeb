using Microsoft.EntityFrameworkCore;

namespace AgriStream.Web.Models
{
    public class AgriStreamContext : DbContext
    {
        public AgriStreamContext(DbContextOptions<AgriStreamContext> options) : base(options) { }

        public DbSet<ApplicationUser> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configure seed data for required system roles: Farmer, Agronomist, Admin
            modelBuilder.Entity<ApplicationUser>().HasData(
                new ApplicationUser
                {
                    Id = 1,
                    Username = "admin_user",
                    PasswordHash = "AQAAAAEAACcQAAAAE...",
                    Role = "Admin"
                },
                new ApplicationUser
                {
                    Id = 2,
                    Username = "farmer_john",
                    PasswordHash = "AQAAAAEAACcQAAAAE...",
                    Role = "Farmer"
                },
                new ApplicationUser
                {
                    Id = 3,
                    Username = "agronomist_sarah",
                    PasswordHash = "AQAAAAEAACcQAAAAE...",
                    Role = "Agronomist"
                }
            );
        }
    }
}