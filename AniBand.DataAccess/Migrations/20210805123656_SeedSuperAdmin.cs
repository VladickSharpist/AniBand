using Microsoft.EntityFrameworkCore.Migrations;

namespace AniBand.DataAccess.Migrations
{
    public partial class SeedSuperAdmin : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
                    Insert into AspNetUsers (RegistrationDate, UserName, NormalizedUserName, Email, NormalizedEmail, EmailConfirmed, PhoneNumberConfirmed, PasswordHash, SecurityStamp, ConcurrencyStamp, TwoFactorEnabled, LockoutEnabled, AccessFailedCount, CreateDate, CreatedById)
                    Values ('01.01.0001', 'SuperAdmin', 'SUPERADMIN', 'SuperAdmin@gmail.com', 'SUPERADMIN@GMAIL.COM', 0, 0, 'AQAAAAEAACcQAAAAEJ/nOdW3Nb8E8/hrX0TRNSzUZ82Xkly19I+F08lmix8wQCixVidBt49NCn7rGQhGxQ==', 'R7FQOV5YWT53ILO4T6BM5ZBKENHEQL4K', '76da7c32-9776-4969-abe7-071adc5b6cfc', 0, 0, 0, '01.01.0001' , (select top 1 u.Id from AspNetUsers u where u.NormalizedEmail='SYSTEM'))
                    Insert into AspNetUserRoles (UserId, RoleId)
                    Values ((select top 1 u.Id from AspNetUsers u where u.NormalizedEmail='SUPERADMIN@GMAIL.COM'), (select top 1 r.Id from AspNetRoles r where r.NormalizedName='SUPERADMIN'))
                    Insert into AspNetUserClaims (UserId, ClaimType, ClaimValue)
                    Values ((select top 1 u.Id from AspNetUsers u where u.NormalizedEmail='SUPERADMIN@GMAIL.COM'), 'AniBand.Permission', '*')
                    ");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
