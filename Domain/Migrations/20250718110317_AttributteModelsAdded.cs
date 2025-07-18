using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Domain.Migrations
{
    /// <inheritdoc />
    public partial class AttributteModelsAdded : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Attributtes",
                columns: table => new
                {
                    AttributteId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AttributteKey = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    AttributeName = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LastModified = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Attributtes", x => x.AttributteId);
                });

            migrationBuilder.CreateTable(
                name: "ProductVariants",
                columns: table => new
                {
                    ProductVariantId = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProductId = table.Column<long>(type: "bigint", nullable: false),
                    SkuNumber = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    PriceAdjustment = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    StockQuantity = table.Column<int>(type: "int", nullable: true),
                    ImageUrl = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    Position = table.Column<int>(type: "int", nullable: false),
                    SupplierId = table.Column<long>(type: "bigint", nullable: true),
                    Status = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    LastModified = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductVariants", x => x.ProductVariantId);
                });

            migrationBuilder.CreateTable(
                name: "AttributteValues",
                columns: table => new
                {
                    AttributeValueId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AttributeValueKey = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    AttributeId = table.Column<int>(type: "int", nullable: false),
                    AttrbtValue = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LastModified = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AttributteValues", x => x.AttributeValueId);
                    table.ForeignKey(
                        name: "FK_AttributteValues_Attributtes_AttributeId",
                        column: x => x.AttributeId,
                        principalTable: "Attributtes",
                        principalColumn: "AttributteId",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateTable(
                name: "ProductVariantAttributes",
                columns: table => new
                {
                    ProductVariantAttributeId = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProductVariantId = table.Column<long>(type: "bigint", nullable: false),
                    AttributeId = table.Column<int>(type: "int", nullable: false),
                    AttributeValueId = table.Column<int>(type: "int", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    LastModified = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductVariantAttributes", x => x.ProductVariantAttributeId);
                    table.ForeignKey(
                        name: "FK_ProductVariantAttributes_AttributteValues_AttributeValueId",
                        column: x => x.AttributeValueId,
                        principalTable: "AttributteValues",
                        principalColumn: "AttributeValueId",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_ProductVariantAttributes_Attributtes_AttributeId",
                        column: x => x.AttributeId,
                        principalTable: "Attributtes",
                        principalColumn: "AttributteId",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_ProductVariantAttributes_ProductVariants_ProductVariantId",
                        column: x => x.ProductVariantId,
                        principalTable: "ProductVariants",
                        principalColumn: "ProductVariantId",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AttributteValues_AttributeId",
                table: "AttributteValues",
                column: "AttributeId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductVariantAttributes_AttributeId",
                table: "ProductVariantAttributes",
                column: "AttributeId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductVariantAttributes_AttributeValueId",
                table: "ProductVariantAttributes",
                column: "AttributeValueId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductVariantAttributes_ProductVariantId",
                table: "ProductVariantAttributes",
                column: "ProductVariantId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ProductVariantAttributes");

            migrationBuilder.DropTable(
                name: "AttributteValues");

            migrationBuilder.DropTable(
                name: "ProductVariants");

            migrationBuilder.DropTable(
                name: "Attributtes");
        }
    }
}
