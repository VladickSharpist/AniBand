using Microsoft.EntityFrameworkCore.Migrations;

namespace AniBand.DataAccess.Migrations
{
    public partial class EposodeAnimeRemane : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Comments_Videos_VideoId",
                table: "Comments");

            migrationBuilder.DropForeignKey(
                name: "FK_Rates_Videos_VideoId",
                table: "Rates");

            migrationBuilder.DropForeignKey(
                name: "FK_Seasons_Studios_StudioId",
                table: "Seasons");

            migrationBuilder.DropForeignKey(
                name: "FK_Videos_Seasons_SeasonId",
                table: "Videos");

            migrationBuilder.DropForeignKey(
                name: "FK_Views_Videos_VideoId",
                table: "Views");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Videos",
                table: "Videos");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Seasons",
                table: "Seasons");

            migrationBuilder.RenameTable(
                name: "Videos",
                newName: "Episodes");

            migrationBuilder.RenameTable(
                name: "Seasons",
                newName: "Animes");

            migrationBuilder.RenameIndex(
                name: "IX_Videos_SeasonId",
                table: "Episodes",
                newName: "IX_Episodes_SeasonId");

            migrationBuilder.RenameIndex(
                name: "IX_Seasons_StudioId",
                table: "Animes",
                newName: "IX_Animes_StudioId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Episodes",
                table: "Episodes",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Animes",
                table: "Animes",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Animes_Studios_StudioId",
                table: "Animes",
                column: "StudioId",
                principalTable: "Studios",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Comments_Episodes_VideoId",
                table: "Comments",
                column: "VideoId",
                principalTable: "Episodes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Episodes_Animes_SeasonId",
                table: "Episodes",
                column: "SeasonId",
                principalTable: "Animes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Rates_Episodes_VideoId",
                table: "Rates",
                column: "VideoId",
                principalTable: "Episodes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Views_Episodes_VideoId",
                table: "Views",
                column: "VideoId",
                principalTable: "Episodes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Animes_Studios_StudioId",
                table: "Animes");

            migrationBuilder.DropForeignKey(
                name: "FK_Comments_Episodes_VideoId",
                table: "Comments");

            migrationBuilder.DropForeignKey(
                name: "FK_Episodes_Animes_SeasonId",
                table: "Episodes");

            migrationBuilder.DropForeignKey(
                name: "FK_Rates_Episodes_VideoId",
                table: "Rates");

            migrationBuilder.DropForeignKey(
                name: "FK_Views_Episodes_VideoId",
                table: "Views");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Episodes",
                table: "Episodes");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Animes",
                table: "Animes");

            migrationBuilder.RenameTable(
                name: "Episodes",
                newName: "Videos");

            migrationBuilder.RenameTable(
                name: "Animes",
                newName: "Seasons");

            migrationBuilder.RenameIndex(
                name: "IX_Episodes_SeasonId",
                table: "Videos",
                newName: "IX_Videos_SeasonId");

            migrationBuilder.RenameIndex(
                name: "IX_Animes_StudioId",
                table: "Seasons",
                newName: "IX_Seasons_StudioId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Videos",
                table: "Videos",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Seasons",
                table: "Seasons",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Comments_Videos_VideoId",
                table: "Comments",
                column: "VideoId",
                principalTable: "Videos",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Rates_Videos_VideoId",
                table: "Rates",
                column: "VideoId",
                principalTable: "Videos",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Seasons_Studios_StudioId",
                table: "Seasons",
                column: "StudioId",
                principalTable: "Studios",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Videos_Seasons_SeasonId",
                table: "Videos",
                column: "SeasonId",
                principalTable: "Seasons",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Views_Videos_VideoId",
                table: "Views",
                column: "VideoId",
                principalTable: "Videos",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
