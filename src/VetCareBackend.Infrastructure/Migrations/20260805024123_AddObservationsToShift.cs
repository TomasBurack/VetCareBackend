using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VetCareBackend.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddObservationsToShift : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Observations",
                table: "Shifts",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Observations",
                table: "Shifts");
        }
    }
}
