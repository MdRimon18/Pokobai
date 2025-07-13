using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Domain.Migrations
{
    /// <inheritdoc />
    public partial class userAddressChaged2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "City",
                table: "UserAddressBooks",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Country",
                table: "UserAddressBooks",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "PhoneNumber",
                table: "UserAddressBooks",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PostalCode",
                table: "UserAddressBooks",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "State",
                table: "UserAddressBooks",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "City",
                table: "UserAddressBooks");

            migrationBuilder.DropColumn(
                name: "Country",
                table: "UserAddressBooks");

            migrationBuilder.DropColumn(
                name: "PhoneNumber",
                table: "UserAddressBooks");

            migrationBuilder.DropColumn(
                name: "PostalCode",
                table: "UserAddressBooks");

            migrationBuilder.DropColumn(
                name: "State",
                table: "UserAddressBooks");
        }
    }
}
