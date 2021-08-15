using AniBand.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AniBand.DataAccess.Configurations
{
    public class SeasonConfiguration : IEntityTypeConfiguration<Season>
    {
        public void Configure(EntityTypeBuilder<Season> builder)
            => builder
                .HasOne(s => s.Studio)
                .WithMany(st => st.Seasons)
                .HasForeignKey(s => s.StudioId);
    }
}