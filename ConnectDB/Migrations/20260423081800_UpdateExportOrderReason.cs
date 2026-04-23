using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ConnectDB.Migrations
{
    /// <inheritdoc />
    public partial class UpdateExportOrderReason : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Type",
                table: "ExportOrders",
                newName: "Status");

            migrationBuilder.AddColumn<string>(
                name: "Reason",
                table: "ExportOrders",
                type: "text",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Reason",
                table: "ExportOrders");

            migrationBuilder.RenameColumn(
                name: "Status",
                table: "ExportOrders",
                newName: "Type");
        }
    }
}
