using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TransactionsService.Migrations
{
    /// <inheritdoc />
    public partial class TranFix : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                schema: "transactions",
                table: "Transactions",
                keyColumn: "Id",
                keyValue: 5L,
                columns: new[] { "AccountId", "Title", "UserId" },
                values: new object[] { 5L, "Some Income", 1L });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                schema: "transactions",
                table: "Transactions",
                keyColumn: "Id",
                keyValue: 5L,
                columns: new[] { "AccountId", "Title", "UserId" },
                values: new object[] { 7L, "Another USER transaction", 2L });
        }
    }
}
