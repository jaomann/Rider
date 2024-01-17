using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using ride_Core.Entities;

namespace ride_Repository
{
    public class Context : IdentityDbContext<IdentityUser>
    {
        public Context(DbContextOptions<Context> options) : base(options)
        {
        }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            SeedRoles(builder);
        }
        private void SeedRoles(ModelBuilder builder)
        {
            builder.Entity<IdentityRole>().HasData(
                new IdentityRole() { Name = "Rider", ConcurrencyStamp = "1", NormalizedName = "Rider" },
                new IdentityRole() { Name = "Passenger", ConcurrencyStamp = "2", NormalizedName = "Passenger" });

        }
        public DbSet<User> Usuarios { get; set; }
    }
}
