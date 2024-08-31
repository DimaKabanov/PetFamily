using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PetFamily.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddValueObjects : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "full_name",
                table: "volunteers",
                newName: "surname");

            migrationBuilder.AddColumn<string>(
                name: "name",
                table: "volunteers",
                type: "character varying(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "patronymic",
                table: "volunteers",
                type: "character varying(50)",
                maxLength: 50,
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "name",
                table: "volunteers");

            migrationBuilder.DropColumn(
                name: "patronymic",
                table: "volunteers");

            migrationBuilder.RenameColumn(
                name: "surname",
                table: "volunteers",
                newName: "full_name");
        }
    }
}
