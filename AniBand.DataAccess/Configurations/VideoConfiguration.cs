using AniBand.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AniBand.DataAccess.Configurations
{
    internal class VideoConfiguration 
        : IEntityTypeConfiguration<Episode>
    {
        public void Configure(EntityTypeBuilder<Episode> builder)
            => builder
                .HasOne(v => v.Anime)
                .WithMany(s => s.Videos)
                .HasForeignKey(v => v.SeasonId);
    }
}