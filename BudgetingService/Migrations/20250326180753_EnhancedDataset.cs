using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BudgetingService.Migrations
{
    /// <inheritdoc />
    public partial class EnhancedDataset : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                schema: "budgets",
                table: "Budgets",
                keyColumn: "Id",
                keyValue: 1L,
                columns: new[] { "CategoryId", "Color" },
                values: new object[] { 4L, "#3f3dbf" });

            migrationBuilder.UpdateData(
                schema: "budgets",
                table: "Budgets",
                keyColumn: "Id",
                keyValue: 2L,
                columns: new[] { "BudgetStatus", "CategoryId", "Color" },
                values: new object[] { 0, 5L, "#34bbb7" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                schema: "budgets",
                table: "Budgets",
                keyColumn: "Id",
                keyValue: 1L,
                columns: new[] { "CategoryId", "Color" },
                values: new object[] { 1L, "#cad6f6" });

            migrationBuilder.UpdateData(
                schema: "budgets",
                table: "Budgets",
                keyColumn: "Id",
                keyValue: 2L,
                columns: new[] { "BudgetStatus", "CategoryId", "Color" },
                values: new object[] { 2, 2L, "#b8a345" });
        }
    }
}
