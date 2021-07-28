using Microsoft.EntityFrameworkCore.Migrations;

namespace AniBand.DataAccess.Migrations
{
    public partial class InsertAdmin : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 1L,
                column: "ConcurrencyStamp",
                value: "1bf377a3-f7b4-4256-86b0-7b821a7f996d");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 2L,
                column: "ConcurrencyStamp",
                value: "9628b642-df84-434b-9305-87dd8f4e3fdd");

            migrationBuilder.Sql(@"
                    Insert into AspNetUsers (Discriminator,RegistrationDate,UserName,NormalizedUserName,Email,NormalizedEmail,EmailConfirmed,PhoneNumberConfirmed,PasswordHash,SecurityStamp,ConcurrencyStamp,TwoFactorEnabled,LockoutEnabled,AccessFailedCount)
                    Values ('User','01.01.0001','Admin','ADMIN','admin@gmail.com','ADMIN@GMAIL.COM',0,0,'AQAAAAEAACcQAAAAEKoWH5DMqLwLAUjGj80cPRt8F7mAgE6WzKqkPfPyjVEC4QsXhqQ4vl5ep8cmY83Y/Q==','GPU7BNMSQENCOYDQC2F3R6HOFKW3YU3T','ac4090a7-9703-4591-a110-c13cba3bd3c9',0,0,0)
                    Insert into AspNetRoleClaims (RoleId,ClaimType,ClaimValue)
                    Values (1,'AniBand.Permission','api.AniBand.Admin.AddVideo')
                    Insert into AspNetRoleClaims (RoleId,ClaimType,ClaimValue)
                    Values (1,'AniBand.Permission','api.AniBand.Admin.RemoveVideo')
                    Insert into AspNetRoleClaims (RoleId,ClaimType,ClaimValue)
                    Values (2,'AniBand.Permission','api.AniBand.User.CommentVideo')
                    Insert into AspNetRoleClaims (RoleId,ClaimType,ClaimValue)
                    Values (2,'AniBand.Permission','api.AniBand.User.WatchVideo')
                    Insert into AspNetUserRoles (UserId,RoleId)
                    Values (1,1)
                    Insert into AspNetUserClaims (UserId,ClaimType,ClaimValue)
                    Values (1,'AniBand.Permission','api.AniBand.Admin.AddVideo')
                    Insert into AspNetUserClaims (UserId,ClaimType,ClaimValue)
                    Values (1,'AniBand.Permission','api.AniBand.Admin.RemoveVideo')
                    ");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 1L,
                column: "ConcurrencyStamp",
                value: "10058cfd-2982-4411-a232-18e766ed421a");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 2L,
                column: "ConcurrencyStamp",
                value: "965eec12-cbef-43a1-ab20-ed89dc9edc46");
        }
    }
}
