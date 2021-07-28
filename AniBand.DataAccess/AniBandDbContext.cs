using AniBand.Domain.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace AniBand.DataAccess
{
    public class AniBandDbContext : IdentityDbContext<IdentityUser<long>, IdentityRole<long>, long>
    {
        public AniBandDbContext(DbContextOptions options) : base(options)
        {

        }

        public DbSet<RefreshToken> RefreshTokensHistory { get; set; }
        
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfiguration(new RoleConfiguration());
        }
        
    }
}

