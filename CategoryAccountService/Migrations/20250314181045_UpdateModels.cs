using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CategoryAccountService.Migrations
{
    /// <inheritdoc />
    public partial class UpdateModels : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "TransactionsCount",
                schema: "category_accounts",
                table: "Accounts",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.UpdateData(
                schema: "category_accounts",
                table: "Accounts",
                keyColumn: "Id",
                keyValue: 1L,
                columns: new[] { "AccountName", "TransactionsCount" },
                values: new object[] { "Bank", 0 });

            migrationBuilder.UpdateData(
                schema: "category_accounts",
                table: "Accounts",
                keyColumn: "Id",
                keyValue: 2L,
                columns: new[] { "AccountName", "TransactionsCount" },
                values: new object[] { "Cash", 0 });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TransactionsCount",
                schema: "category_accounts",
                table: "Accounts");

            migrationBuilder.UpdateData(
                schema: "category_accounts",
                table: "Accounts",
                keyColumn: "Id",
                keyValue: 1L,
                column: "AccountName",
                value: "Checking Account");

            migrationBuilder.UpdateData(
                schema: "category_accounts",
                table: "Accounts",
                keyColumn: "Id",
                keyValue: 2L,
                column: "AccountName",
                value: "Savings Account");
        }
    }
}
