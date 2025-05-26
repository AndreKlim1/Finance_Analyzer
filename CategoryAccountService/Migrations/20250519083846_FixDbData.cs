using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CategoryAccountService.Migrations
{
    /// <inheritdoc />
    public partial class FixDbData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                schema: "category_accounts",
                table: "Accounts",
                keyColumn: "Id",
                keyValue: 7L);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                schema: "category_accounts",
                table: "Accounts",
                columns: new[] { "Id", "AccountName", "AccountType", "Balance", "Color", "Currency", "Description", "TransactionsCount", "UserId" },
                values: new object[] { 7L, "Other USER bank", 1, 3000m, "#7d4aad", 0, "Long-term savings account", 0, 2L });
        }
    }
}
