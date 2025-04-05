using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace CategoryAccountService.Migrations
{
    /// <inheritdoc />
    public partial class EnhancedDataset : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                schema: "category_accounts",
                table: "Categories",
                keyColumn: "Id",
                keyValue: 1L,
                columns: new[] { "CategoryName", "CategoryType", "Icon", "UserId" },
                values: new object[] { "Transfer", 2, "/assets/exchange-arrows.png", null });

            migrationBuilder.UpdateData(
                schema: "category_accounts",
                table: "Categories",
                keyColumn: "Id",
                keyValue: 2L,
                columns: new[] { "CategoryName", "CategoryType", "Icon", "UserId" },
                values: new object[] { "Correction", 3, "/assets/increase.png", null });

            migrationBuilder.InsertData(
                schema: "category_accounts",
                table: "Categories",
                columns: new[] { "Id", "CategoryName", "CategoryType", "Icon", "UserId" },
                values: new object[,]
                {
                    { 3L, "Salary", 0, "/assets/categories/bills.png", null },
                    { 4L, "Shopping", 1, "/assets/categories/shopping.png", null },
                    { 5L, "Random money", 0, "/assets/categories/wallet.png", null },
                    { 6L, "Food", 1, "/assets/categories/curry.png", null }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                schema: "category_accounts",
                table: "Categories",
                keyColumn: "Id",
                keyValue: 3L);

            migrationBuilder.DeleteData(
                schema: "category_accounts",
                table: "Categories",
                keyColumn: "Id",
                keyValue: 4L);

            migrationBuilder.DeleteData(
                schema: "category_accounts",
                table: "Categories",
                keyColumn: "Id",
                keyValue: 5L);

            migrationBuilder.DeleteData(
                schema: "category_accounts",
                table: "Categories",
                keyColumn: "Id",
                keyValue: 6L);

            migrationBuilder.UpdateData(
                schema: "category_accounts",
                table: "Categories",
                keyColumn: "Id",
                keyValue: 1L,
                columns: new[] { "CategoryName", "CategoryType", "Icon", "UserId" },
                values: new object[] { "Groceries", 1, "", 1L });

            migrationBuilder.UpdateData(
                schema: "category_accounts",
                table: "Categories",
                keyColumn: "Id",
                keyValue: 2L,
                columns: new[] { "CategoryName", "CategoryType", "Icon", "UserId" },
                values: new object[] { "Salary", 0, "", 2L });
        }
    }
}
