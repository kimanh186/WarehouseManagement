using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ConnectDB.Migrations
{
    /// <inheritdoc />
    public partial class AddWarehouseTables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ExportOrderDetails_ExportOrders_ExportOrderId",
                table: "ExportOrderDetails");

            migrationBuilder.DropForeignKey(
                name: "FK_ImportOrderDetails_ImportOrders_ImportOrderId",
                table: "ImportOrderDetails");

            migrationBuilder.AlterColumn<string>(
                name: "Phone",
                table: "Suppliers",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.CreateIndex(
                name: "IX_Suppliers_Phone",
                table: "Suppliers",
                column: "Phone",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Products_ProductCode",
                table: "Products",
                column: "ProductCode",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_ExportOrderDetails_ExportOrders_ExportOrderId",
                table: "ExportOrderDetails",
                column: "ExportOrderId",
                principalTable: "ExportOrders",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ImportOrderDetails_ImportOrders_ImportOrderId",
                table: "ImportOrderDetails",
                column: "ImportOrderId",
                principalTable: "ImportOrders",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ExportOrderDetails_ExportOrders_ExportOrderId",
                table: "ExportOrderDetails");

            migrationBuilder.DropForeignKey(
                name: "FK_ImportOrderDetails_ImportOrders_ImportOrderId",
                table: "ImportOrderDetails");

            migrationBuilder.DropIndex(
                name: "IX_Suppliers_Phone",
                table: "Suppliers");

            migrationBuilder.DropIndex(
                name: "IX_Products_ProductCode",
                table: "Products");

            migrationBuilder.AlterColumn<string>(
                name: "Phone",
                table: "Suppliers",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AddForeignKey(
                name: "FK_ExportOrderDetails_ExportOrders_ExportOrderId",
                table: "ExportOrderDetails",
                column: "ExportOrderId",
                principalTable: "ExportOrders",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ImportOrderDetails_ImportOrders_ImportOrderId",
                table: "ImportOrderDetails",
                column: "ImportOrderId",
                principalTable: "ImportOrders",
                principalColumn: "Id");
        }
    }
}
