using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PetFamily.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UpdateJsonbColumnName : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Details",
                table: "volunteers",
                newName: "details");

            migrationBuilder.RenameColumn(
                name: "Details",
                table: "pets",
                newName: "details");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "details",
                table: "volunteers",
                newName: "Details");

            migrationBuilder.RenameColumn(
                name: "details",
                table: "pets",
                newName: "Details");
        }
    }
}
