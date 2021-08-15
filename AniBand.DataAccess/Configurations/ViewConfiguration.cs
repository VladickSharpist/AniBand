using AniBand.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AniBand.DataAccess.Configurations
{
    public class ViewConfiguration : IEntityTypeConfiguration<View>
    {
        public void Configure(EntityTypeBuilder<View> builder)
        {
            builder
                .HasOne(v => v.User)
                .WithMany(u => u.Views)
                .HasForeignKey(v => v.UserId);

            builder
                .HasOne(v => v.Video)
                .WithMany(vid => vid.Views)
                .HasForeignKey(v => v.VideoId);
        }
    }
}