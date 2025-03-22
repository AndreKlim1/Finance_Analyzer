using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BudgetingService.Migrations
{
    /// <inheritdoc />
    public partial class AccountIdMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "AccountId",
                schema: "budgets",
                table: "Budgets",
                type: "bigint",
                nullable: true);

            migrationBuilder.UpdateData(
                schema: "budgets",
                table: "Budgets",
                keyColumn: "Id",
                keyValue: 1L,
                column: "AccountId",
                value: 1L);

            migrationBuilder.UpdateData(
                schema: "budgets",
                table: "Budgets",
                keyColumn: "Id",
                keyValue: 2L,
                column: "AccountId",
                value: 2L);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AccountId",
                schema: "budgets",
                table: "Budgets");
        }
    }
}
