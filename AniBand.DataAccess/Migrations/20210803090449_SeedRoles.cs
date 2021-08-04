using Microsoft.EntityFrameworkCore.Migrations;

namespace AniBand.DataAccess.Migrations
{
    public partial class SeedRoles : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
                    Insert into AspNetRoles (Name, NormalizedName)
                    Values ('System','SYSTEM')
                    Insert into AspNetRoles (Name, NormalizedName, ConcurrencyStamp)
                    Values ('Admin','ADMIN','02e7ccbb-f040-4dc7-adc2-e4cd40f6fdab')
                    Insert into AspNetRoles (Name, NormalizedName, ConcurrencyStamp)
                    Values ('User','USER','acb3bf33-82dc-48eb-837e-a9b7aef3c8e0')
                    Insert into AspNetRoleClaims (RoleId, ClaimType, ClaimValue)
                    Values ((select top 1 r.Id from AspNetRoles r where r.NormalizedName='ADMIN'), 'AniBand.Permission', 'api.AniBand.Admin.AddVideo')
                    Insert into AspNetRoleClaims (RoleId, ClaimType, ClaimValue)
                    Values ((select top 1 r.Id from AspNetRoles r where r.NormalizedName='ADMIN'), 'AniBand.Permission', 'api.AniBand.Admin.RemoveVideo')
                    Insert into AspNetRoleClaims (RoleId, ClaimType, ClaimValue)
                    Values ((select top 1 r.Id from AspNetRoles r where r.NormalizedName='USER'), 'AniBand.Permission', 'api.AniBand.User.CommentVideo')
                    Insert into AspNetRoleClaims (RoleId, ClaimType, ClaimValue)
                    Values ((select top 1 r.Id from AspNetRoles r where r.NormalizedName='USER'), 'AniBand.Permission', 'api.AniBand.User.WatchVideo')
            ");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
