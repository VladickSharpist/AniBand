using AniBand.Domain;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AniBand.DataAccess
{
    class RoleConfiguration : IEntityTypeConfiguration<IdentityRole<long>>
    {
        public void Configure(EntityTypeBuilder<IdentityRole<long>> builder)
        {
            builder.HasData(new IdentityRole<long>
                {
                    Id = 1, 
                    Name = Roles.Admin.ToString(), 
                    NormalizedName = Roles.Admin.ToString().ToUpper()
                },
                new IdentityRole<long>
                {
                    Id = 2, 
                    Name = Roles.User.ToString(), 
                    NormalizedName = Roles.User.ToString().ToUpper()
                });
        }
    }
}