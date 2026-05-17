using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VetCareBackend.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class DeletedUserIdInVeterinarianTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Veterinarians_UserId",
                table: "Veterinarians");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Veterinarians");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "UserId",
                table: "Veterinarians",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_Veterinarians_UserId",
                table: "Veterinarians",
                column: "UserId");
        }
    }
}
