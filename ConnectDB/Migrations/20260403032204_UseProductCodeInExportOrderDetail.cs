using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ConnectDB.Migrations
{
    /// <inheritdoc />
    public partial class UseProductCodeInExportOrderDetail : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ExportOrderDetails_Products_ProductId",
                table: "ExportOrderDetails");

            migrationBuilder.DropIndex(
                name: "IX_ExportOrderDetails_ProductId",
                table: "ExportOrderDetails");

            migrationBuilder.DropColumn(
                name: "ProductId",
                table: "ExportOrderDetails");

            migrationBuilder.AddColumn<string>(
                name: "ProductCode",
                table: "ExportOrderDetails",
                type: "nvarchar(50)",
                nullable: false,
                defaultValue: "SP001");

            migrationBuilder.CreateIndex(
                name: "IX_ExportOrderDetails_ProductCode",
                table: "ExportOrderDetails",
                column: "ProductCode");

            migrationBuilder.AddForeignKey(
                name: "FK_ExportOrderDetails_Products_ProductCode",
                table: "ExportOrderDetails",
                column: "ProductCode",
                principalTable: "Products",
                principalColumn: "ProductCode",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ExportOrderDetails_Products_ProductCode",
                table: "ExportOrderDetails");

            migrationBuilder.DropIndex(
                name: "IX_ExportOrderDetails_ProductCode",
                table: "ExportOrderDetails");

            migrationBuilder.DropColumn(
                name: "ProductCode",
                table: "ExportOrderDetails");

            migrationBuilder.AddColumn<int>(
                name: "ProductId",
                table: "ExportOrderDetails",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_ExportOrderDetails_ProductId",
                table: "ExportOrderDetails",
                column: "ProductId");

            migrationBuilder.AddForeignKey(
                name: "FK_ExportOrderDetails_Products_ProductId",
                table: "ExportOrderDetails",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
