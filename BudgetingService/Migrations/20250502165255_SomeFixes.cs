using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BudgetingService.Migrations
{
    /// <inheritdoc />
    public partial class SomeFixes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                schema: "budgets",
                table: "Budgets",
                keyColumn: "Id",
                keyValue: 1L,
                columns: new[] { "AccountIds", "CategoryIds" },
                values: new object[] { new[] { 1L }, new[] { 4L, 6L } });

            migrationBuilder.UpdateData(
                schema: "budgets",
                table: "Budgets",
                keyColumn: "Id",
                keyValue: 2L,
                columns: new[] { "AccountIds", "CategoryIds" },
                values: new object[] { new[] { 2L, 4L }, new[] { 3L, 5L } });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                schema: "budgets",
                table: "Budgets",
                keyColumn: "Id",
                keyValue: 1L,
                columns: new[] { "AccountIds", "CategoryIds" },
                values: new object[] { new List<long> { 1L }, new List<long> { 4L, 6L } });

            migrationBuilder.UpdateData(
                schema: "budgets",
                table: "Budgets",
                keyColumn: "Id",
                keyValue: 2L,
                columns: new[] { "AccountIds", "CategoryIds" },
                values: new object[] { new List<long> { 2L, 4L }, new List<long> { 3L, 5L } });
        }
    }
}
