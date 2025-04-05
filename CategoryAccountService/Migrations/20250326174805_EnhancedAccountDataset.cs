using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CategoryAccountService.Migrations
{
    /// <inheritdoc />
    public partial class EnhancedAccountDataset : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                schema: "category_accounts",
                table: "Accounts",
                keyColumn: "Id",
                keyValue: 1L,
                column: "Color",
                value: "#be6464");

            migrationBuilder.UpdateData(
                schema: "category_accounts",
                table: "Accounts",
                keyColumn: "Id",
                keyValue: 2L,
                column: "Color",
                value: "#187547");

            migrationBuilder.InsertData(
                schema: "category_accounts",
                table: "Accounts",
                columns: new[] { "Id", "AccountName", "AccountType", "Balance", "Color", "Currency", "Description", "TransactionsCount", "UserId" },
                values: new object[] { 3L, "Second Cash", 1, 3000, "#6572bd", 0, "Long-term savings account", 0, 1L });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                schema: "category_accounts",
                table: "Accounts",
                keyColumn: "Id",
                keyValue: 3L);

            migrationBuilder.UpdateData(
                schema: "category_accounts",
                table: "Accounts",
                keyColumn: "Id",
                keyValue: 1L,
                column: "Color",
                value: "#b14242");

            migrationBuilder.UpdateData(
                schema: "category_accounts",
                table: "Accounts",
                keyColumn: "Id",
                keyValue: 2L,
                column: "Color",
                value: "#33cf5a");
        }
    }
}
