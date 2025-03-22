using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TransactionsService.Migrations
{
    /// <inheritdoc />
    public partial class UpdateModels : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Title",
                schema: "transactions",
                table: "Transactions",
                type: "character varying(128)",
                maxLength: 128,
                nullable: false,
                defaultValue: "");

            migrationBuilder.UpdateData(
                schema: "transactions",
                table: "Transactions",
                keyColumn: "Id",
                keyValue: 1L,
                column: "Title",
                value: "First transaction");

            migrationBuilder.UpdateData(
                schema: "transactions",
                table: "Transactions",
                keyColumn: "Id",
                keyValue: 2L,
                column: "Title",
                value: "Second transaction");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Title",
                schema: "transactions",
                table: "Transactions");
        }
    }
}
