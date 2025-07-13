using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Domain.Migrations
{
    /// <inheritdoc />
    public partial class SupplierNdCustomerModelCreated : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Customers",
                columns: table => new
                {
                    CustomerId = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BranchId = table.Column<long>(type: "bigint", nullable: true),
                    CustomerName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    MobileNo = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Email = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    CountryId = table.Column<long>(type: "bigint", nullable: false),
                    CountryCode = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    StateName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    CustAddrssOne = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false),
                    CustAddrssTwo = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false),
                    Occupation = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    OfficeName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    CustImgLink = table.Column<string>(type: "nvarchar(180)", maxLength: 180, nullable: false),
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
                    table.PrimaryKey("PK_Customers", x => x.CustomerId);
                });

            migrationBuilder.CreateTable(
                name: "Suppliers",
                columns: table => new
                {
                    SupplierId = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BranchId = table.Column<long>(type: "bigint", nullable: true),
                    SupplrName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    MobileNo = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SuppOrgnznName = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    CountryId = table.Column<long>(type: "bigint", nullable: false),
                    CountryCode = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    StateName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    BusinessTypeId = table.Column<long>(type: "bigint", nullable: false),
                    SupplrAddrssOne = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SupplrAddrssTwo = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DeliveryOffDay = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SupplrImgLink = table.Column<string>(type: "nvarchar(max)", nullable: true),
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
                    table.PrimaryKey("PK_Suppliers", x => x.SupplierId);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Customers");

            migrationBuilder.DropTable(
                name: "Suppliers");
        }
    }
}
