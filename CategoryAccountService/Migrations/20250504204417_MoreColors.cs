using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CategoryAccountService.Migrations
{
    /// <inheritdoc />
    public partial class MoreColors : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                schema: "category_accounts",
                table: "Categories",
                keyColumn: "Id",
                keyValue: 3L,
                column: "Color",
                value: "#187547");

            migrationBuilder.UpdateData(
                schema: "category_accounts",
                table: "Categories",
                keyColumn: "Id",
                keyValue: 4L,
                column: "Color",
                value: "#ccc670");

            migrationBuilder.UpdateData(
                schema: "category_accounts",
                table: "Categories",
                keyColumn: "Id",
                keyValue: 5L,
                column: "Color",
                value: "#9ab950");

            migrationBuilder.UpdateData(
                schema: "category_accounts",
                table: "Categories",
                keyColumn: "Id",
                keyValue: 6L,
                column: "Color",
                value: "#be6464");

            migrationBuilder.UpdateData(
                schema: "category_accounts",
                table: "Categories",
                keyColumn: "Id",
                keyValue: 7L,
                column: "Color",
                value: "#da7c8b");

            migrationBuilder.UpdateData(
                schema: "category_accounts",
                table: "Categories",
                keyColumn: "Id",
                keyValue: 8L,
                column: "Color",
                value: "#6d6d7a");

            migrationBuilder.UpdateData(
                schema: "category_accounts",
                table: "Categories",
                keyColumn: "Id",
                keyValue: 9L,
                column: "Color",
                value: "#7d4aad");

            migrationBuilder.UpdateData(
                schema: "category_accounts",
                table: "Categories",
                keyColumn: "Id",
                keyValue: 10L,
                column: "Color",
                value: "#da7c8b");

            migrationBuilder.UpdateData(
                schema: "category_accounts",
                table: "Categories",
                keyColumn: "Id",
                keyValue: 11L,
                column: "Color",
                value: "#9ab950");

            migrationBuilder.UpdateData(
                schema: "category_accounts",
                table: "Categories",
                keyColumn: "Id",
                keyValue: 12L,
                column: "Color",
                value: "#d6a458");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                schema: "category_accounts",
                table: "Categories",
                keyColumn: "Id",
                keyValue: 3L,
                column: "Color",
                value: "#6572bd");

            migrationBuilder.UpdateData(
                schema: "category_accounts",
                table: "Categories",
                keyColumn: "Id",
                keyValue: 4L,
                column: "Color",
                value: "#6572bd");

            migrationBuilder.UpdateData(
                schema: "category_accounts",
                table: "Categories",
                keyColumn: "Id",
                keyValue: 5L,
                column: "Color",
                value: "#6572bd");

            migrationBuilder.UpdateData(
                schema: "category_accounts",
                table: "Categories",
                keyColumn: "Id",
                keyValue: 6L,
                column: "Color",
                value: "#6572bd");

            migrationBuilder.UpdateData(
                schema: "category_accounts",
                table: "Categories",
                keyColumn: "Id",
                keyValue: 7L,
                column: "Color",
                value: "#6572bd");

            migrationBuilder.UpdateData(
                schema: "category_accounts",
                table: "Categories",
                keyColumn: "Id",
                keyValue: 8L,
                column: "Color",
                value: "#6572bd");

            migrationBuilder.UpdateData(
                schema: "category_accounts",
                table: "Categories",
                keyColumn: "Id",
                keyValue: 9L,
                column: "Color",
                value: "#6572bd");

            migrationBuilder.UpdateData(
                schema: "category_accounts",
                table: "Categories",
                keyColumn: "Id",
                keyValue: 10L,
                column: "Color",
                value: "#6572bd");

            migrationBuilder.UpdateData(
                schema: "category_accounts",
                table: "Categories",
                keyColumn: "Id",
                keyValue: 11L,
                column: "Color",
                value: "#6572bd");

            migrationBuilder.UpdateData(
                schema: "category_accounts",
                table: "Categories",
                keyColumn: "Id",
                keyValue: 12L,
                column: "Color",
                value: "#6572bd");
        }
    }
}
