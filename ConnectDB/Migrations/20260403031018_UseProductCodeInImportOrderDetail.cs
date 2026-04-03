using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ConnectDB.Migrations
{
    /// <inheritdoc />
    public partial class UseProductCodeInImportOrderDetail : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ImportOrderDetails_Products_ProductId",
                table: "ImportOrderDetails");

            migrationBuilder.DropIndex(
                name: "IX_ImportOrderDetails_ProductId",
                table: "ImportOrderDetails");

            migrationBuilder.DropColumn(
                name: "ProductId",
                table: "ImportOrderDetails");

            migrationBuilder.AddColumn<string>(
                name: "ProductCode",
                table: "ImportOrderDetails",
                type: "nvarchar(50)",
                nullable: false,
                defaultValue: "SP001");

            migrationBuilder.AddUniqueConstraint(
                name: "AK_Products_ProductCode",
                table: "Products",
                column: "ProductCode");

            migrationBuilder.CreateIndex(
                name: "IX_ImportOrderDetails_ProductCode",
                table: "ImportOrderDetails",
                column: "ProductCode");

            migrationBuilder.AddForeignKey(
                name: "FK_ImportOrderDetails_Products_ProductCode",
                table: "ImportOrderDetails",
                column: "ProductCode",
                principalTable: "Products",
                principalColumn: "ProductCode",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ImportOrderDetails_Products_ProductCode",
                table: "ImportOrderDetails");

            migrationBuilder.DropUniqueConstraint(
                name: "AK_Products_ProductCode",
                table: "Products");

            migrationBuilder.DropIndex(
                name: "IX_ImportOrderDetails_ProductCode",
                table: "ImportOrderDetails");

            migrationBuilder.DropColumn(
                name: "ProductCode",
                table: "ImportOrderDetails");

            migrationBuilder.AddColumn<int>(
                name: "ProductId",
                table: "ImportOrderDetails",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_ImportOrderDetails_ProductId",
                table: "ImportOrderDetails",
                column: "ProductId");

            migrationBuilder.AddForeignKey(
                name: "FK_ImportOrderDetails_Products_ProductId",
                table: "ImportOrderDetails",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
