using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AniBand.Auth.Services.Abstractions.Services;
using AniBand.Domain.Interfaces;
using AniBand.Domain.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace AniBand.DataAccess
{
    public class AniBandDbContext : IdentityDbContext<
        User, 
        IdentityRole<long>, 
        long,
        IdentityUserClaim<long>,
        IdentityUserRole<long>,
        IdentityUserLogin<long>,
        IdentityRoleClaim<long>,
        UserToken>
    {
        private readonly IUserAccessor _userAccessor;

        public AniBandDbContext(DbContextOptions options,
            IUserAccessor userAccessor) 
            : base(options)
        {
            _userAccessor = userAccessor;
        }

        public DbSet<RefreshToken> RefreshTokensHistory { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<RefreshToken>()
                .HasOne(t => t.Owner)
                .WithMany(u => u.RefreshTokensHistory)
                .HasForeignKey(t => t.OwnerId);

            base.OnModelCreating(modelBuilder);
        }

        public override async Task<int> SaveChangesAsync(
            bool acceptAllChangesOnSuccess,
            CancellationToken cancellationToken = new CancellationToken())
        {
            OnBeforeSaving();
            return await base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
        }

        public override int SaveChanges(bool acceptAllChangesOnSuccess)
        {
            OnBeforeSaving();
            return base.SaveChanges(acceptAllChangesOnSuccess);
        }

        private void OnBeforeSaving()
        {
            var entries = ChangeTracker.Entries();
            
            var user = _userAccessor.GetUser();
            var actorId = user?.Id 
                          ?? Users.Single(u 
                                  => u.NormalizedEmail == "SYSTEM").Id;
            
            foreach (var entry in entries)
            {
                switch (entry.State)
                {
                    case EntityState.Added:
                        
                        if (entry.Entity is ICreatableEntity creatableEntity)
                        {
                            creatableEntity.CreateDate = DateTime.Now;
                            creatableEntity.CreatedById = actorId;
                        }

                        break;
                    
                    case EntityState.Modified:
                        
                        if (entry.Entity is IUpdatableEntity updatableEntity)
                        {
                            updatableEntity.UpdateDate = DateTime.Now;
                            updatableEntity.UpdatedById = actorId;
                        }

                        break;
                }
            }
        }
    }
}