using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PetFamily.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class RemovePetDetails : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "details",
                table: "pets",
                newName: "requisite_list");

            migrationBuilder.AddColumn<string>(
                name: "photo_list",
                table: "pets",
                type: "jsonb",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "photo_list",
                table: "pets");

            migrationBuilder.RenameColumn(
                name: "requisite_list",
                table: "pets",
                newName: "details");
        }
    }
}
