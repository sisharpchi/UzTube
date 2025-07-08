using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Persistence.Migrations.AppDb
{
    /// <inheritdoc />
    public partial class InitMainDb : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_LikeDislikes_Videos_VideoId",
                table: "LikeDislikes");

            migrationBuilder.DropForeignKey(
                name: "FK_Videos_Playlists_PlaylistId",
                table: "Videos");

            migrationBuilder.AddForeignKey(
                name: "FK_LikeDislikes_Videos_VideoId",
                table: "LikeDislikes",
                column: "VideoId",
                principalTable: "Videos",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Videos_Playlists_PlaylistId",
                table: "Videos",
                column: "PlaylistId",
                principalTable: "Playlists",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_LikeDislikes_Videos_VideoId",
                table: "LikeDislikes");

            migrationBuilder.DropForeignKey(
                name: "FK_Videos_Playlists_PlaylistId",
                table: "Videos");

            migrationBuilder.AddForeignKey(
                name: "FK_LikeDislikes_Videos_VideoId",
                table: "LikeDislikes",
                column: "VideoId",
                principalTable: "Videos",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Videos_Playlists_PlaylistId",
                table: "Videos",
                column: "PlaylistId",
                principalTable: "Playlists",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
