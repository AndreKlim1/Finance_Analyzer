using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CategoryAccountService.Migrations
{
    /// <inheritdoc />
    public partial class SomeLittleFixes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<decimal>(
                name: "Balance",
                schema: "category_accounts",
                table: "Accounts",
                type: "numeric",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.UpdateData(
                schema: "category_accounts",
                table: "Accounts",
                keyColumn: "Id",
                keyValue: 1L,
                column: "Balance",
                value: 1500m);

            migrationBuilder.UpdateData(
                schema: "category_accounts",
                table: "Accounts",
                keyColumn: "Id",
                keyValue: 2L,
                column: "Balance",
                value: 5000m);

            migrationBuilder.UpdateData(
                schema: "category_accounts",
                table: "Accounts",
                keyColumn: "Id",
                keyValue: 3L,
                column: "Balance",
                value: 3000m);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "Balance",
                schema: "category_accounts",
                table: "Accounts",
                type: "integer",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "numeric");

            migrationBuilder.UpdateData(
                schema: "category_accounts",
                table: "Accounts",
                keyColumn: "Id",
                keyValue: 1L,
                column: "Balance",
                value: 1500);

            migrationBuilder.UpdateData(
                schema: "category_accounts",
                table: "Accounts",
                keyColumn: "Id",
                keyValue: 2L,
                column: "Balance",
                value: 5000);

            migrationBuilder.UpdateData(
                schema: "category_accounts",
                table: "Accounts",
                keyColumn: "Id",
                keyValue: 3L,
                column: "Balance",
                value: 3000);
        }
    }
}
