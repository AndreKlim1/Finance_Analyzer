using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BudgetingService.Migrations
{
    /// <inheritdoc />
    public partial class MoreInitialData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AccountId",
                schema: "budgets",
                table: "Budgets");

            migrationBuilder.DropColumn(
                name: "CategoryId",
                schema: "budgets",
                table: "Budgets");

            migrationBuilder.AddColumn<List<long>>(
                name: "AccountIds",
                schema: "budgets",
                table: "Budgets",
                type: "bigint[]",
                nullable: true);

            migrationBuilder.AddColumn<List<long>>(
                name: "CategoryIds",
                schema: "budgets",
                table: "Budgets",
                type: "bigint[]",
                nullable: true);

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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AccountIds",
                schema: "budgets",
                table: "Budgets");

            migrationBuilder.DropColumn(
                name: "CategoryIds",
                schema: "budgets",
                table: "Budgets");

            migrationBuilder.AddColumn<long>(
                name: "AccountId",
                schema: "budgets",
                table: "Budgets",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "CategoryId",
                schema: "budgets",
                table: "Budgets",
                type: "bigint",
                nullable: true);

            migrationBuilder.UpdateData(
                schema: "budgets",
                table: "Budgets",
                keyColumn: "Id",
                keyValue: 1L,
                columns: new[] { "AccountId", "CategoryId" },
                values: new object[] { 1L, 4L });

            migrationBuilder.UpdateData(
                schema: "budgets",
                table: "Budgets",
                keyColumn: "Id",
                keyValue: 2L,
                columns: new[] { "AccountId", "CategoryId" },
                values: new object[] { 2L, 5L });
        }
    }
}
