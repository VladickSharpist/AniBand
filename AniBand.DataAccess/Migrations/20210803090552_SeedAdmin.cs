using Microsoft.EntityFrameworkCore.Migrations;

namespace AniBand.DataAccess.Migrations
{
    public partial class SeedAdmin : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
                    Insert into AspNetUsers (RegistrationDate, UserName, NormalizedUserName, Email, NormalizedEmail, EmailConfirmed, PhoneNumberConfirmed, PasswordHash, SecurityStamp, ConcurrencyStamp, TwoFactorEnabled, LockoutEnabled, AccessFailedCount,CreateDate, CreatedById)
                    Values ('01.01.0001', 'Admin', 'ADMIN', 'admin@gmail.com', 'ADMIN@GMAIL.COM', 0, 0,'AQAAAAEAACcQAAAAEKoWH5DMqLwLAUjGj80cPRt8F7mAgE6WzKqkPfPyjVEC4QsXhqQ4vl5ep8cmY83Y/Q==', 'GPU7BNMSQENCOYDQC2F3R6HOFKW3YU3T', 'ac4090a7-9703-4591-a110-c13cba3bd3c9', 0, 0, 0, '01.01.0001' , (select top 1 u.Id from AspNetUsers u where u.NormalizedEmail='SYSTEM'))
                    Insert into AspNetUserRoles (UserId, RoleId)
                    Values ((select top 1 u.Id from AspNetUsers u where u.NormalizedEmail='ADMIN@GMAIL.COM'), (select top 1 u.Id from AspNetRoles u where u.NormalizedName='ADMIN'))
                    Insert into AspNetUserClaims (UserId, ClaimType, ClaimValue)
                    Values ((select top 1 u.Id from AspNetUsers u where u.NormalizedEmail='ADMIN@GMAIL.COM'), 'AniBand.Permission', 'api.AniBand.Admin.AddVideo')
                    Insert into AspNetUserClaims (UserId, ClaimType, ClaimValue)
                    Values ((select top 1 u.Id from AspNetUsers u where u.NormalizedEmail='ADMIN@GMAIL.COM'), 'AniBand.Permission', 'api.AniBand.Admin.RemoveVideo')
            ");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
