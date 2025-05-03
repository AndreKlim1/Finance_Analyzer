using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace CategoryAccountService.Migrations
{
    /// <inheritdoc />
    public partial class MoreInitialData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Color",
                schema: "category_accounts",
                table: "Categories",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.UpdateData(
                schema: "category_accounts",
                table: "Accounts",
                keyColumn: "Id",
                keyValue: 2L,
                column: "UserId",
                value: 1L);

            migrationBuilder.InsertData(
                schema: "category_accounts",
                table: "Accounts",
                columns: new[] { "Id", "AccountName", "AccountType", "Balance", "Color", "Currency", "Description", "TransactionsCount", "UserId" },
                values: new object[,]
                {
                    { 4L, "Another bank", 0, 3000m, "#7d4aad", 0, "Long-term savings account", 0, 1L },
                    { 5L, "For black day", 1, 400m, "#ccc670", 1, "Long-term savings account", 0, 1L },
                    { 6L, "Free money", 0, 500m, "#80bed6", 0, "Long-term savings account", 0, 1L },
                    { 7L, "Other USER bank", 1, 3000m, "#7d4aad", 0, "Long-term savings account", 0, 2L }
                });

            migrationBuilder.UpdateData(
                schema: "category_accounts",
                table: "Categories",
                keyColumn: "Id",
                keyValue: 1L,
                column: "Color",
                value: "#6572bd");

            migrationBuilder.UpdateData(
                schema: "category_accounts",
                table: "Categories",
                keyColumn: "Id",
                keyValue: 2L,
                columns: new[] { "Color", "Icon" },
                values: new object[] { "#6572bd", "/assets/categories/settings.png" });

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
                columns: new[] { "CategoryName", "Color" },
                values: new object[] { "Restaurants", "#6572bd" });

            migrationBuilder.InsertData(
                schema: "category_accounts",
                table: "Categories",
                columns: new[] { "Id", "CategoryName", "CategoryType", "Color", "Icon", "UserId" },
                values: new object[,]
                {
                    { 7L, "Passive gain", 0, "#6572bd", "/assets/increase.png", null },
                    { 8L, "Other sources", 0, "#6572bd", "/assets/categories/code.png", null },
                    { 9L, "Clothes", 1, "#6572bd", "/assets/categories/tshirt.png", null },
                    { 10L, "Entertainment", 1, "#6572bd", "/assets/categories/popcorn.png", null },
                    { 11L, "Foodstuff", 1, "#6572bd", "/assets/categories/shopping-cart.png", null },
                    { 12L, "Home expenses", 1, "#6572bd", "/assets/categories/house.png", null }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                schema: "category_accounts",
                table: "Accounts",
                keyColumn: "Id",
                keyValue: 4L);

            migrationBuilder.DeleteData(
                schema: "category_accounts",
                table: "Accounts",
                keyColumn: "Id",
                keyValue: 5L);

            migrationBuilder.DeleteData(
                schema: "category_accounts",
                table: "Accounts",
                keyColumn: "Id",
                keyValue: 6L);

            migrationBuilder.DeleteData(
                schema: "category_accounts",
                table: "Accounts",
                keyColumn: "Id",
                keyValue: 7L);

            migrationBuilder.DeleteData(
                schema: "category_accounts",
                table: "Categories",
                keyColumn: "Id",
                keyValue: 7L);

            migrationBuilder.DeleteData(
                schema: "category_accounts",
                table: "Categories",
                keyColumn: "Id",
                keyValue: 8L);

            migrationBuilder.DeleteData(
                schema: "category_accounts",
                table: "Categories",
                keyColumn: "Id",
                keyValue: 9L);

            migrationBuilder.DeleteData(
                schema: "category_accounts",
                table: "Categories",
                keyColumn: "Id",
                keyValue: 10L);

            migrationBuilder.DeleteData(
                schema: "category_accounts",
                table: "Categories",
                keyColumn: "Id",
                keyValue: 11L);

            migrationBuilder.DeleteData(
                schema: "category_accounts",
                table: "Categories",
                keyColumn: "Id",
                keyValue: 12L);

            migrationBuilder.DropColumn(
                name: "Color",
                schema: "category_accounts",
                table: "Categories");

            migrationBuilder.UpdateData(
                schema: "category_accounts",
                table: "Accounts",
                keyColumn: "Id",
                keyValue: 2L,
                column: "UserId",
                value: 2L);

            migrationBuilder.UpdateData(
                schema: "category_accounts",
                table: "Categories",
                keyColumn: "Id",
                keyValue: 2L,
                column: "Icon",
                value: "/assets/increase.png");

            migrationBuilder.UpdateData(
                schema: "category_accounts",
                table: "Categories",
                keyColumn: "Id",
                keyValue: 6L,
                column: "CategoryName",
                value: "Food");
        }
    }
}
