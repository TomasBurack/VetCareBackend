using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VetCareBackend.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class TypeConsultchangestoSpecialitytypeConsultremovedfromshiftandspecialityaddedinveterinarian : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TypeConsult",
                table: "Shifts");

            migrationBuilder.AddColumn<int>(
                name: "Speciality",
                table: "Veterinarians",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Speciality",
                table: "Veterinarians");

            migrationBuilder.AddColumn<int>(
                name: "TypeConsult",
                table: "Shifts",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
