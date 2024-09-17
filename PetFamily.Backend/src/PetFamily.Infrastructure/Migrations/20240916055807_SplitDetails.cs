using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PetFamily.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class SplitDetails : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "details",
                table: "volunteers",
                newName: "social_network_list");

            migrationBuilder.AddColumn<string>(
                name: "requisite_list",
                table: "volunteers",
                type: "jsonb",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "requisite_list",
                table: "volunteers");

            migrationBuilder.RenameColumn(
                name: "social_network_list",
                table: "volunteers",
                newName: "details");
        }
    }
}
