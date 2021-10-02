using AniBand.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AniBand.DataAccess.Configurations
{
    internal class SeasonConfiguration 
        : IEntityTypeConfiguration<Anime>
    {
        public void Configure(EntityTypeBuilder<Anime> builder)
            => builder
                .HasOne(s => s.Studio)
                .WithMany(st => st.Seasons)
                .HasForeignKey(s => s.StudioId);
    }
}