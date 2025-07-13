using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Domain.Migrations
{
    /// <inheritdoc />
    public partial class UserModelAdded : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "PhoneNumber",
                table: "UserAddressBooks",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldMaxLength: 100,
                oldNullable: true);

            migrationBuilder.CreateTable(
                name: "UserDetails",
                columns: table => new
                {
                    UserDetailId = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<long>(type: "bigint", nullable: false),
                    PhoneTwo = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    EmailTwo = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    NidNo = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    PassportNo = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    OrganizationName = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    Occupation = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    OfficeName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    BusinessTypeId = table.Column<long>(type: "bigint", nullable: true),
                    OffDay = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    OrgImgLink = table.Column<string>(type: "nvarchar(180)", maxLength: 180, nullable: true),
                    NumberOfStaff = table.Column<int>(type: "int", nullable: false),
                    Note = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserDetails", x => x.UserDetailId);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    UserId = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PhoneNo = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Password = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RoleId = table.Column<long>(type: "bigint", nullable: false),
                    CompanyId = table.Column<long>(type: "bigint", nullable: true),
                    BranchId = table.Column<long>(type: "bigint", nullable: true),
                    CountryId = table.Column<long>(type: "bigint", nullable: true),
                    CountryCode = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    MembershipId = table.Column<int>(type: "int", maxLength: 100, nullable: true),
                    IsAbleToLogin = table.Column<bool>(type: "bit", nullable: false),
                    ImgLink = table.Column<string>(type: "nvarchar(max)", nullable: true),
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
                    table.PrimaryKey("PK_Users", x => x.UserId);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UserDetails");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.AlterColumn<string>(
                name: "PhoneNumber",
                table: "UserAddressBooks",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);
        }
    }
}
