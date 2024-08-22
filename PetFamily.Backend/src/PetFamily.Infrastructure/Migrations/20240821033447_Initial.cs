using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PetFamily.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Volunteers",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    full_name = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    description = table.Column<string>(type: "character varying(2000)", maxLength: 2000, nullable: false),
                    experience = table.Column<int>(type: "integer", nullable: false),
                    phone = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_volunteers", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "Pets",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    name = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    type = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    description = table.Column<string>(type: "character varying(2000)", maxLength: 2000, nullable: false),
                    breed = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    color = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    health = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    address = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: false),
                    weight = table.Column<double>(type: "double precision", nullable: false),
                    height = table.Column<double>(type: "double precision", nullable: false),
                    phone = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    is_castrated = table.Column<bool>(type: "boolean", nullable: false),
                    date_of_birth = table.Column<DateOnly>(type: "date", nullable: false),
                    is_vaccinated = table.Column<bool>(type: "boolean", nullable: false),
                    assistance_status = table.Column<int>(type: "integer", nullable: false),
                    created_date = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    volunteer_id = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_pets", x => x.id);
                    table.ForeignKey(
                        name: "fk_pets_volunteers_volunteer_id",
                        column: x => x.volunteer_id,
                        principalTable: "Volunteers",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "SocialNetworks",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    title = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    url = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    volunteer_id = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_social_networks", x => x.id);
                    table.ForeignKey(
                        name: "fk_social_networks_volunteers_volunteer_id",
                        column: x => x.volunteer_id,
                        principalTable: "Volunteers",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "PetPhotos",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    path = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: false),
                    is_main = table.Column<bool>(type: "boolean", nullable: false),
                    pet_id = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_pet_photos", x => x.id);
                    table.ForeignKey(
                        name: "fk_pet_photos_pets_pet_id",
                        column: x => x.pet_id,
                        principalTable: "Pets",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "Requisites",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    name = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    description = table.Column<string>(type: "character varying(2000)", maxLength: 2000, nullable: false),
                    pet_id = table.Column<Guid>(type: "uuid", nullable: true),
                    volunteer_id = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_requisites", x => x.id);
                    table.ForeignKey(
                        name: "fk_requisites_pets_pet_id",
                        column: x => x.pet_id,
                        principalTable: "Pets",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "fk_requisites_volunteers_volunteer_id",
                        column: x => x.volunteer_id,
                        principalTable: "Volunteers",
                        principalColumn: "id");
                });

            migrationBuilder.CreateIndex(
                name: "ix_pet_photos_pet_id",
                table: "PetPhotos",
                column: "pet_id");

            migrationBuilder.CreateIndex(
                name: "ix_pets_volunteer_id",
                table: "Pets",
                column: "volunteer_id");

            migrationBuilder.CreateIndex(
                name: "ix_requisites_pet_id",
                table: "Requisites",
                column: "pet_id");

            migrationBuilder.CreateIndex(
                name: "ix_requisites_volunteer_id",
                table: "Requisites",
                column: "volunteer_id");

            migrationBuilder.CreateIndex(
                name: "ix_social_networks_volunteer_id",
                table: "SocialNetworks",
                column: "volunteer_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PetPhotos");

            migrationBuilder.DropTable(
                name: "Requisites");

            migrationBuilder.DropTable(
                name: "SocialNetworks");

            migrationBuilder.DropTable(
                name: "Pets");

            migrationBuilder.DropTable(
                name: "Volunteers");
        }
    }
}
