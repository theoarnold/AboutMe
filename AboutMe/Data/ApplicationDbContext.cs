using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace AboutMe.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public DbSet<ApplicationInfo> ApplicationInfos { get; set; }
        public DbSet<ButtonInfo> ButtonInfos { get; set; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            // Configure ApplicationInfo entity
            builder.Entity<ApplicationInfo>()
                .HasData(new ApplicationInfo
                {
                    Id = 1,
                    Bio = "Log in to the dashboard to modify the biography section.",
                    GithubCred = "This should be your GitHub credentials.",
                    GithubName = "This should be your GitHub username",
                    // Set other properties accordingly
                });

            // Configure ButtonInfo entity
            builder.Entity<ButtonInfo>()
                .HasData(new ButtonInfo
                {
                    Id = 1,
                    ButtonText = "Default Button",
                    ButtonColourHex = "#ff0097",
                    ButtonUrl = "https://github.com/theoarnold",
                    ApplicationInfoId = 1,
                    // Set other properties accordingly
                });

            // Configure the relationship between ApplicationInfo and ButtonInfo
            builder.Entity<ApplicationInfo>()
                .HasMany(a => a.Buttons)
                .WithOne(b => b.ApplicationInfo)
                .HasForeignKey(b => b.ApplicationInfoId)
                .IsRequired(false)
                .OnDelete(DeleteBehavior.Cascade);

            // Seed data for additional ButtonInfo entities if needed
            builder.Entity<ButtonInfo>()
                .HasData(new ButtonInfo
                {
                    Id = 2,
                    ButtonText = "Another Button",
                    ButtonColourHex = "#00ff00",
                    ButtonUrl = "https://example.com",
                    ApplicationInfoId = 1,
                    // Set other properties accordingly
                });

            // (You may add more ButtonInfo entities if needed)
        }
    }
}
