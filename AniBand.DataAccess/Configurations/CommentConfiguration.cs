using AniBand.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AniBand.DataAccess.Configurations
{
    internal class CommentConfiguration 
        : IEntityTypeConfiguration<Comment>
    {
        public void Configure(EntityTypeBuilder<Comment> builder)
        {
            builder
                .HasOne(c => c.User)
                .WithMany(u => u.Comments)
                .HasForeignKey(c => c.UserId);
            
            builder
                .HasOne(c => c.Episode)
                .WithMany(v => v.Comments)
                .HasForeignKey(c => c.VideoId);
        }
    }
}