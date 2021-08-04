using Microsoft.EntityFrameworkCore.Migrations;

namespace AniBand.DataAccess.Migrations
{
    public partial class SeedSystem : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
                    Insert into AspNetUsers (UserName, NormalizedUserName, Email, NormalizedEmail, EmailConfirmed, PhoneNumberConfirmed, PasswordHash, SecurityStamp, ConcurrencyStamp, TwoFactorEnabled, LockoutEnabled, AccessFailedCount, CreateDate, RegistrationDate, CreatedById)
                    Values ('System', 'SYSTEM', 'System', 'SYSTEM', 1, 1,'AQAAAAEAACcQAAAAEAEWwws6/5hi7fSRX+i2JnIXpEQlMnawj2Nmhys75yGG2qBk1+HilZvw3afha3AC/w==', 'UVG4ECAHX5XUCOGLQZYBFMXJXHN3E34X', 'e71e1d05-38b7-46c5-843c-097ac6857fb7', 0, 0, 0, '01.01.0001', '01.01.0001', 0)
                    Insert into AspNetUserRoles (UserId, RoleId)
                    Values ((select top 1 u.Id from AspNetUsers u where u.NormalizedEmail='SYSTEM'), (select top 1 u.Id from AspNetRoles u where u.NormalizedName='SYSTEM'))
                    Update AspNetUsers 
                    Set CreatedById = (select top 1 u.Id from AspNetUsers u where u.NormalizedEmail='SYSTEM')
                    Where Id = (select top 1 u.Id from AspNetUsers u where u.NormalizedEmail='SYSTEM')
            ");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
