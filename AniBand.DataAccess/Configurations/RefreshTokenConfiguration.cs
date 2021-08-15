using AniBand.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AniBand.DataAccess.Configurations
{
    public class RefreshTokenConfiguration : IEntityTypeConfiguration<RefreshToken>
    {
        public void Configure(EntityTypeBuilder<RefreshToken> builder)
            => builder
                .HasOne(t => t.Owner)
                .WithMany(u => u.RefreshTokensHistory)
                .HasForeignKey(t => t.OwnerId);
    }
}