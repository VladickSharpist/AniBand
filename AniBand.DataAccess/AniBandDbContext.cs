using System;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using AniBand.Auth.Services.Abstractions.Services;
using AniBand.Domain.Abstractions.Interfaces;
using AniBand.Domain.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace AniBand.DataAccess
{
    public class AniBandDbContext 
        : IdentityDbContext<
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

        private const string SYSTEM_NORMALIZED_EMAIL = "SYSTEM";
        
        public AniBandDbContext(
            DbContextOptions options,
            IUserAccessor userAccessor) 
            : base(options)
        {
            _userAccessor = userAccessor 
                ?? throw new NullReferenceException(nameof(userAccessor));
        }

        public DbSet<RefreshToken> RefreshTokensHistory { get; set; }
        public DbSet<Studio> Studios { get; set; }
        public DbSet<Anime> Animes { get; set; }
        public DbSet<Episode> Episodes { get; set; }
        public DbSet<Rate> Rates { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<View> Views { get; set; }
        public DbSet<Notification> Notifications { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
            
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
            var user = _userAccessor.User;
            var actorId = user?.Id 
                          ?? Users.Single(u 
                                  => u.NormalizedEmail == SYSTEM_NORMALIZED_EMAIL).Id;

            SaveUpdatableEntities(actorId);
            SaveCreatableEntities(actorId);
        }

        private void SaveUpdatableEntities(long actorId)
        {
            foreach (var entry in ChangeTracker
                .Entries()
                .Where(
                    e => e.Entity is IUpdatableEntity
                         && e.State == EntityState.Modified)
                .ToList())
            {
                ((IUpdatableEntity) entry.Entity).UpdateDate = DateTime.Now;
                ((IUpdatableEntity) entry.Entity).UpdatedById = actorId;
            }
        }

        private void SaveCreatableEntities(long actorId)
        {
            foreach (var entry in ChangeTracker
                .Entries()
                .Where(
                    e => e.Entity is ICreatableEntity
                         && e.State == EntityState.Added)
                .ToList())
            {
                ((ICreatableEntity) entry.Entity).CreateDate = DateTime.Now;
                ((ICreatableEntity) entry.Entity).CreatedById = actorId;
            }
        }
    }
}