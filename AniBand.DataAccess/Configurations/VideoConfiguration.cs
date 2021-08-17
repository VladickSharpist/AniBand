using AniBand.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AniBand.DataAccess.Configurations
{
    internal class VideoConfiguration 
        : IEntityTypeConfiguration<Video>
    {
        public void Configure(EntityTypeBuilder<Video> builder)
            => builder
                .HasOne(v => v.Season)
                .WithMany(s => s.Videos)
                .HasForeignKey(v => v.SeasonId);
    }
}