using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VetCareBackend.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class RemoveTwoFactorRecoveryCodes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TwoFactorRecoveryCodesHash",
                table: "Veterinarians");

            migrationBuilder.DropColumn(
                name: "TwoFactorRecoveryCodesHash",
                table: "Sysadmins");

            migrationBuilder.DropColumn(
                name: "TwoFactorRecoveryCodesHash",
                table: "Clients");

            migrationBuilder.DropColumn(
                name: "TwoFactorRecoveryCodesHash",
                table: "Administrators");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "TwoFactorRecoveryCodesHash",
                table: "Veterinarians",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TwoFactorRecoveryCodesHash",
                table: "Sysadmins",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TwoFactorRecoveryCodesHash",
                table: "Clients",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TwoFactorRecoveryCodesHash",
                table: "Administrators",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
