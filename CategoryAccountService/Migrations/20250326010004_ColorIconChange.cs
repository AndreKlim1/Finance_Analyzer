using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CategoryAccountService.Migrations
{
    /// <inheritdoc />
    public partial class ColorIconChange : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Icon",
                schema: "category_accounts",
                table: "Categories",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Color",
                schema: "category_accounts",
                table: "Accounts",
                type: "character varying(32)",
                maxLength: 32,
                nullable: false,
                defaultValue: "");

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

            migrationBuilder.UpdateData(
                schema: "category_accounts",
                table: "Categories",
                keyColumn: "Id",
                keyValue: 1L,
                column: "Icon",
                value: "");

            migrationBuilder.UpdateData(
                schema: "category_accounts",
                table: "Categories",
                keyColumn: "Id",
                keyValue: 2L,
                column: "Icon",
                value: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Icon",
                schema: "category_accounts",
                table: "Categories");

            migrationBuilder.DropColumn(
                name: "Color",
                schema: "category_accounts",
                table: "Accounts");
        }
    }
}
