using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Domain.Migrations
{
    /// <inheritdoc />
    public partial class UserPhoneNumbersAdded : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "key",
                table: "UserAddressBooks",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateTable(
                name: "UserPhoneNumbers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<long>(type: "bigint", nullable: false),
                    PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    key = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    EntryDateTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    EntryBy = table.Column<long>(type: "bigint", nullable: true),
                    LastModifyDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifyBy = table.Column<long>(type: "bigint", nullable: true),
                    DeletedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedBy = table.Column<long>(type: "bigint", nullable: true),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserPhoneNumbers", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UserPhoneNumbers");

            migrationBuilder.DropColumn(
                name: "key",
                table: "UserAddressBooks");
        }
    }
}
