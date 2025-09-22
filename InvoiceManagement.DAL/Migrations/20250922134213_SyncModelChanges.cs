using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace InvoiceManagement.DAL.Migrations
{
    /// <inheritdoc />
    public partial class SyncModelChanges : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LineTotal",
                table: "InvoiceLines");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "LineTotal",
                table: "InvoiceLines",
                type: "TEXT",
                nullable: false,
                defaultValue: 0m);
        }
    }
}
