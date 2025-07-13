using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Domain.Migrations
{
    /// <inheritdoc />
    public partial class UserAddressBook : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BillingPlans");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.CreateTable(
                name: "UserAddressBooks",
                columns: table => new
                {
                    AddressID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserID = table.Column<long>(type: "bigint", nullable: false),
                    AddressType = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Address = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    IsDefault = table.Column<bool>(type: "bit", nullable: false),
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
                    table.PrimaryKey("PK_UserAddressBooks", x => x.AddressID);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UserAddressBooks");

            migrationBuilder.CreateTable(
                name: "BillingPlans",
                columns: table => new
                {
                    BillingPlanId = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BillingPlanKey = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    BillingPlanName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DeletedBy = table.Column<long>(type: "bigint", nullable: true),
                    DeletedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    EntryBy = table.Column<long>(type: "bigint", nullable: true),
                    EntryDateTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LanguageId = table.Column<int>(type: "int", nullable: false),
                    LastModifyBy = table.Column<long>(type: "bigint", nullable: true),
                    LastModifyDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    total_row = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BillingPlans", x => x.BillingPlanId);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    UserId = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DeletedBy = table.Column<long>(type: "bigint", nullable: true),
                    DeletedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    EntryBy = table.Column<long>(type: "bigint", nullable: true),
                    EntryDateTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifyBy = table.Column<long>(type: "bigint", nullable: true),
                    LastModifyDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UserDesignation = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UserImgLink = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UserKey = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    UserName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UserPassword = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UserPhoneNo = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    total_row = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.UserId);
                });
        }
    }
}
