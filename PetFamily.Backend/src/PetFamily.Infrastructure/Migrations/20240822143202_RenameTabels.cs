using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PetFamily.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class RenameTabels : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameTable(
                name: "Volunteers",
                newName: "volunteers");

            migrationBuilder.RenameTable(
                name: "Requisites",
                newName: "requisites");

            migrationBuilder.RenameTable(
                name: "Pets",
                newName: "pets");

            migrationBuilder.RenameTable(
                name: "SocialNetworks",
                newName: "social_networks");

            migrationBuilder.RenameTable(
                name: "PetPhotos",
                newName: "pet_photos");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameTable(
                name: "volunteers",
                newName: "Volunteers");

            migrationBuilder.RenameTable(
                name: "requisites",
                newName: "Requisites");

            migrationBuilder.RenameTable(
                name: "pets",
                newName: "Pets");

            migrationBuilder.RenameTable(
                name: "social_networks",
                newName: "SocialNetworks");

            migrationBuilder.RenameTable(
                name: "pet_photos",
                newName: "PetPhotos");
        }
    }
}
