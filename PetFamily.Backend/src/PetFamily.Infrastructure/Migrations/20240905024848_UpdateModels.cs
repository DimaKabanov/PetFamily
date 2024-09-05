using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PetFamily.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UpdateModels : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "address",
                table: "pets",
                newName: "street");

            migrationBuilder.AddColumn<int>(
                name: "flat",
                table: "pets",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "home",
                table: "pets",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "flat",
                table: "pets");

            migrationBuilder.DropColumn(
                name: "home",
                table: "pets");

            migrationBuilder.RenameColumn(
                name: "street",
                table: "pets",
                newName: "address");
        }
    }
}
