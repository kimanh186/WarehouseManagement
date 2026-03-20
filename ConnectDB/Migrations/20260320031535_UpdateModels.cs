using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ConnectDB.Migrations
{
    /// <inheritdoc />
    public partial class UpdateModels : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ExportOrders",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Type = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ExportOrders", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Suppliers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SupplierName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Phone = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Suppliers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ExportOrderDetails",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProductId = table.Column<int>(type: "int", nullable: false),
                    Quantity = table.Column<int>(type: "int", nullable: false),
                    ExportOrderId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ExportOrderDetails", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ExportOrderDetails_ExportOrders_ExportOrderId",
                        column: x => x.ExportOrderId,
                        principalTable: "ExportOrders",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_ExportOrderDetails_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ImportOrders",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    SupplierId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ImportOrders", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ImportOrders_Suppliers_SupplierId",
                        column: x => x.SupplierId,
                        principalTable: "Suppliers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ImportOrderDetails",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProductId = table.Column<int>(type: "int", nullable: false),
                    Quantity = table.Column<int>(type: "int", nullable: false),
                    ImportPrice = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    ImportOrderId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ImportOrderDetails", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ImportOrderDetails_ImportOrders_ImportOrderId",
                        column: x => x.ImportOrderId,
                        principalTable: "ImportOrders",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_ImportOrderDetails_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ExportOrderDetails_ExportOrderId",
                table: "ExportOrderDetails",
                column: "ExportOrderId");

            migrationBuilder.CreateIndex(
                name: "IX_ExportOrderDetails_ProductId",
                table: "ExportOrderDetails",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_ImportOrderDetails_ImportOrderId",
                table: "ImportOrderDetails",
                column: "ImportOrderId");

            migrationBuilder.CreateIndex(
                name: "IX_ImportOrderDetails_ProductId",
                table: "ImportOrderDetails",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_ImportOrders_SupplierId",
                table: "ImportOrders",
                column: "SupplierId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ExportOrderDetails");

            migrationBuilder.DropTable(
                name: "ImportOrderDetails");

            migrationBuilder.DropTable(
                name: "ExportOrders");

            migrationBuilder.DropTable(
                name: "ImportOrders");

            migrationBuilder.DropTable(
                name: "Suppliers");
        }
    }
}
