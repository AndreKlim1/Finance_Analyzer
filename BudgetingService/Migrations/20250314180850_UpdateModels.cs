using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BudgetingService.Migrations
{
    /// <inheritdoc />
    public partial class UpdateModels : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CurrValue",
                schema: "budgets",
                table: "Budgets",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<bool>(
                name: "WarningShowed",
                schema: "budgets",
                table: "Budgets",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.UpdateData(
                schema: "budgets",
                table: "Budgets",
                keyColumn: "Id",
                keyValue: 1L,
                columns: new[] { "CurrValue", "WarningShowed" },
                values: new object[] { 100, false });

            migrationBuilder.UpdateData(
                schema: "budgets",
                table: "Budgets",
                keyColumn: "Id",
                keyValue: 2L,
                columns: new[] { "CurrValue", "WarningShowed" },
                values: new object[] { 200, false });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CurrValue",
                schema: "budgets",
                table: "Budgets");

            migrationBuilder.DropColumn(
                name: "WarningShowed",
                schema: "budgets",
                table: "Budgets");
        }
    }
}
