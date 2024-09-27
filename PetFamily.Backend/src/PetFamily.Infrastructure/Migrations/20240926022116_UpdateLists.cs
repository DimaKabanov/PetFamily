using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PetFamily.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UpdateLists : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "social_network_list",
                table: "volunteers",
                newName: "social_networks");

            migrationBuilder.RenameColumn(
                name: "requisite_list",
                table: "volunteers",
                newName: "requisites");

            migrationBuilder.RenameColumn(
                name: "requisite_list",
                table: "pets",
                newName: "requisites");

            migrationBuilder.RenameColumn(
                name: "photo_list",
                table: "pets",
                newName: "photos");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "social_networks",
                table: "volunteers",
                newName: "social_network_list");

            migrationBuilder.RenameColumn(
                name: "requisites",
                table: "volunteers",
                newName: "requisite_list");

            migrationBuilder.RenameColumn(
                name: "requisites",
                table: "pets",
                newName: "requisite_list");

            migrationBuilder.RenameColumn(
                name: "photos",
                table: "pets",
                newName: "photo_list");
        }
    }
}
