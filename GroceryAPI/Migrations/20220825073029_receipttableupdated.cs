using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GroceryAPI.Migrations
{
    public partial class receipttableupdated : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_receipts_payments_PaymentID",
                table: "receipts");

            migrationBuilder.DropIndex(
                name: "IX_receipts_PaymentID",
                table: "receipts");

            migrationBuilder.DropColumn(
                name: "PaymentID",
                table: "receipts");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "PaymentID",
                table: "receipts",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_receipts_PaymentID",
                table: "receipts",
                column: "PaymentID");

            migrationBuilder.AddForeignKey(
                name: "FK_receipts_payments_PaymentID",
                table: "receipts",
                column: "PaymentID",
                principalTable: "payments",
                principalColumn: "PaymentID");
        }
    }
}
