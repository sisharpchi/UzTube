using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Data.Migrations.AppDb
{
    /// <inheritdoc />
    public partial class Initial2AuthMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "AvatarCloudPublicId",
                table: "Channels",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "AvatarUrl",
                table: "Channels",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "BannerCloudPublicId",
                table: "Channels",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "BannerUrl",
                table: "Channels",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AvatarCloudPublicId",
                table: "Channels");

            migrationBuilder.DropColumn(
                name: "AvatarUrl",
                table: "Channels");

            migrationBuilder.DropColumn(
                name: "BannerCloudPublicId",
                table: "Channels");

            migrationBuilder.DropColumn(
                name: "BannerUrl",
                table: "Channels");
        }
    }
}
