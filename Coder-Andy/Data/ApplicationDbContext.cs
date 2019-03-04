using CoderAndy.Models.Blog;
using CoderAndy.Models.Media;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace CoderAndy.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public DbSet<Post> Posts { get; set; }
        public DbSet<Category> Categories { get; set; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> a_options)
            : base(a_options)
        {
            
        }

        protected override void OnModelCreating(ModelBuilder a_builder)
        {
            base.OnModelCreating(a_builder);

            // Shorten key length for Identity
            a_builder.Entity<IdentityUser>(entity => entity.Property(m => m.Id).HasMaxLength(127));
            a_builder.Entity<IdentityRole>(entity => entity.Property(m => m.Id).HasMaxLength(127));
            a_builder.Entity<IdentityUserLogin<string>>(entity =>
            {
                entity.Property(m => m.LoginProvider).HasMaxLength(127);
                entity.Property(m => m.ProviderKey).HasMaxLength(127);
            });
            a_builder.Entity<IdentityUserRole<string>>(entity =>
            {
                entity.Property(m => m.UserId).HasMaxLength(127);
                entity.Property(m => m.RoleId).HasMaxLength(127);
            });
            a_builder.Entity<IdentityUserToken<string>>(entity =>
            {
                entity.Property(m => m.UserId).HasMaxLength(127);
                entity.Property(m => m.LoginProvider).HasMaxLength(127);
                entity.Property(m => m.Name).HasMaxLength(127);
            });
        }
    }
}
