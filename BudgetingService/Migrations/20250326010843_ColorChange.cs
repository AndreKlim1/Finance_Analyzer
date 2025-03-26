using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BudgetingService.Migrations
{
    /// <inheritdoc />
    public partial class ColorChange : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Color",
                schema: "budgets",
                table: "Budgets",
                type: "character varying(32)",
                maxLength: 32,
                nullable: false,
                defaultValue: "");

            migrationBuilder.UpdateData(
                schema: "budgets",
                table: "Budgets",
                keyColumn: "Id",
                keyValue: 1L,
                column: "Color",
                value: "#cad6f6");

            migrationBuilder.UpdateData(
                schema: "budgets",
                table: "Budgets",
                keyColumn: "Id",
                keyValue: 2L,
                column: "Color",
                value: "#b8a345");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Color",
                schema: "budgets",
                table: "Budgets");
        }
    }
}
