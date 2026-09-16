using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VetCareBackend.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddTwoFactorAuthenticationToUser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "TwoFactorEnabled",
                table: "Veterinarians",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "TwoFactorRecoveryCodesHash",
                table: "Veterinarians",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TwoFactorSecret",
                table: "Veterinarians",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "TwoFactorEnabled",
                table: "Sysadmins",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "TwoFactorRecoveryCodesHash",
                table: "Sysadmins",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TwoFactorSecret",
                table: "Sysadmins",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "TwoFactorEnabled",
                table: "Clients",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "TwoFactorRecoveryCodesHash",
                table: "Clients",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TwoFactorSecret",
                table: "Clients",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "TwoFactorEnabled",
                table: "Administrators",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "TwoFactorRecoveryCodesHash",
                table: "Administrators",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TwoFactorSecret",
                table: "Administrators",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TwoFactorEnabled",
                table: "Veterinarians");

            migrationBuilder.DropColumn(
                name: "TwoFactorRecoveryCodesHash",
                table: "Veterinarians");

            migrationBuilder.DropColumn(
                name: "TwoFactorSecret",
                table: "Veterinarians");

            migrationBuilder.DropColumn(
                name: "TwoFactorEnabled",
                table: "Sysadmins");

            migrationBuilder.DropColumn(
                name: "TwoFactorRecoveryCodesHash",
                table: "Sysadmins");

            migrationBuilder.DropColumn(
                name: "TwoFactorSecret",
                table: "Sysadmins");

            migrationBuilder.DropColumn(
                name: "TwoFactorEnabled",
                table: "Clients");

            migrationBuilder.DropColumn(
                name: "TwoFactorRecoveryCodesHash",
                table: "Clients");

            migrationBuilder.DropColumn(
                name: "TwoFactorSecret",
                table: "Clients");

            migrationBuilder.DropColumn(
                name: "TwoFactorEnabled",
                table: "Administrators");

            migrationBuilder.DropColumn(
                name: "TwoFactorRecoveryCodesHash",
                table: "Administrators");

            migrationBuilder.DropColumn(
                name: "TwoFactorSecret",
                table: "Administrators");
        }
    }
}
