using AniBand.Domain.Enums;
using Microsoft.EntityFrameworkCore.Migrations;

namespace AniBand.DataAccess.Migrations
{
    public partial class CommentStatus : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "DeclineMessage",
                table: "Comments",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Status",
                table: "Comments",
                type: "int",
                nullable: false,
                defaultValue: Status.Waiting);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DeclineMessage",
                table: "Comments");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "Comments");
        }
    }
}
