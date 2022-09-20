using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GroceryAPI.Migrations
{
    public partial class receipttableupdate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_receipts_payments_PaymentID",
                table: "receipts");

            migrationBuilder.RenameColumn(
                name: "TotalAmount",
                table: "receipts",
                newName: "Amount");

            migrationBuilder.AlterColumn<int>(
                name: "PaymentID",
                table: "receipts",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<int>(
                name: "GroceryID",
                table: "receipts",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Quantity",
                table: "receipts",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_receipts_GroceryID",
                table: "receipts",
                column: "GroceryID");

            migrationBuilder.AddForeignKey(
                name: "FK_receipts_grocery_GroceryID",
                table: "receipts",
                column: "GroceryID",
                principalTable: "grocery",
                principalColumn: "GroceryID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_receipts_payments_PaymentID",
                table: "receipts",
                column: "PaymentID",
                principalTable: "payments",
                principalColumn: "PaymentID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_receipts_grocery_GroceryID",
                table: "receipts");

            migrationBuilder.DropForeignKey(
                name: "FK_receipts_payments_PaymentID",
                table: "receipts");

            migrationBuilder.DropIndex(
                name: "IX_receipts_GroceryID",
                table: "receipts");

            migrationBuilder.DropColumn(
                name: "GroceryID",
                table: "receipts");

            migrationBuilder.DropColumn(
                name: "Quantity",
                table: "receipts");

            migrationBuilder.RenameColumn(
                name: "Amount",
                table: "receipts",
                newName: "TotalAmount");

            migrationBuilder.AlterColumn<int>(
                name: "PaymentID",
                table: "receipts",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_receipts_payments_PaymentID",
                table: "receipts",
                column: "PaymentID",
                principalTable: "payments",
                principalColumn: "PaymentID",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
