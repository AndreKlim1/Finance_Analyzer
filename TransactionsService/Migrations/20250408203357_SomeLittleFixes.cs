using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TransactionsService.Migrations
{
    /// <inheritdoc />
    public partial class SomeLittleFixes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<decimal>(
                name: "Value",
                schema: "transactions",
                table: "Transactions",
                type: "numeric",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.UpdateData(
                schema: "transactions",
                table: "Transactions",
                keyColumn: "Id",
                keyValue: 1L,
                column: "Value",
                value: 100m);

            migrationBuilder.UpdateData(
                schema: "transactions",
                table: "Transactions",
                keyColumn: "Id",
                keyValue: 2L,
                column: "Value",
                value: -200m);

            migrationBuilder.UpdateData(
                schema: "transactions",
                table: "Transactions",
                keyColumn: "Id",
                keyValue: 3L,
                column: "Value",
                value: -400m);

            migrationBuilder.UpdateData(
                schema: "transactions",
                table: "Transactions",
                keyColumn: "Id",
                keyValue: 4L,
                column: "Value",
                value: 100m);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "Value",
                schema: "transactions",
                table: "Transactions",
                type: "integer",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "numeric");

            migrationBuilder.UpdateData(
                schema: "transactions",
                table: "Transactions",
                keyColumn: "Id",
                keyValue: 1L,
                column: "Value",
                value: 100);

            migrationBuilder.UpdateData(
                schema: "transactions",
                table: "Transactions",
                keyColumn: "Id",
                keyValue: 2L,
                column: "Value",
                value: -200);

            migrationBuilder.UpdateData(
                schema: "transactions",
                table: "Transactions",
                keyColumn: "Id",
                keyValue: 3L,
                column: "Value",
                value: -400);

            migrationBuilder.UpdateData(
                schema: "transactions",
                table: "Transactions",
                keyColumn: "Id",
                keyValue: 4L,
                column: "Value",
                value: 100);
        }
    }
}
