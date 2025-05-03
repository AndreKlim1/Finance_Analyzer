using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace TransactionsService.Migrations
{
    /// <inheritdoc />
    public partial class MoreInitialData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                schema: "transactions",
                table: "Transactions",
                keyColumn: "Id",
                keyValue: 1L,
                columns: new[] { "CreationDate", "TransactionDate", "Value" },
                values: new object[] { new DateTime(2025, 4, 7, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2025, 4, 7, 0, 0, 0, 0, DateTimeKind.Utc), 400m });

            migrationBuilder.UpdateData(
                schema: "transactions",
                table: "Transactions",
                keyColumn: "Id",
                keyValue: 2L,
                columns: new[] { "CreationDate", "Title", "TransactionDate", "UserId" },
                values: new object[] { new DateTime(2025, 4, 8, 0, 0, 0, 0, DateTimeKind.Utc), "Something for home", new DateTime(2025, 4, 8, 0, 0, 0, 0, DateTimeKind.Utc), 1L });

            migrationBuilder.UpdateData(
                schema: "transactions",
                table: "Transactions",
                keyColumn: "Id",
                keyValue: 3L,
                columns: new[] { "CreationDate", "TransactionDate", "UserId" },
                values: new object[] { new DateTime(2025, 4, 10, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2025, 4, 10, 0, 0, 0, 0, DateTimeKind.Utc), 1L });

            migrationBuilder.UpdateData(
                schema: "transactions",
                table: "Transactions",
                keyColumn: "Id",
                keyValue: 4L,
                columns: new[] { "CreationDate", "TransactionDate", "UserId" },
                values: new object[] { new DateTime(2025, 4, 12, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2025, 4, 12, 0, 0, 0, 0, DateTimeKind.Utc), 1L });

            migrationBuilder.InsertData(
                schema: "transactions",
                table: "Transactions",
                columns: new[] { "Id", "AccountId", "CategoryId", "CreationDate", "Currency", "Description", "Image", "Merchant", "Title", "TransactionDate", "TransactionType", "UserId", "Value" },
                values: new object[,]
                {
                    { 5L, 7L, 3L, new DateTime(2025, 4, 7, 0, 0, 0, 0, DateTimeKind.Utc), 1, "Test transaction 1", null, "Amazon", "Another USER transaction", new DateTime(2025, 4, 7, 0, 0, 0, 0, DateTimeKind.Utc), 0, 2L, 100m },
                    { 6L, 1L, 3L, new DateTime(2025, 4, 14, 0, 0, 0, 0, DateTimeKind.Utc), 1, "Test transaction 1", null, "Amazon", "Finally Salary", new DateTime(2025, 4, 14, 0, 0, 0, 0, DateTimeKind.Utc), 0, 1L, 1100m },
                    { 7L, 4L, 9L, new DateTime(2025, 4, 15, 0, 0, 0, 0, DateTimeKind.Utc), 1, "Test transaction 1", null, "Amazon", "New Tshirt", new DateTime(2025, 4, 15, 0, 0, 0, 0, DateTimeKind.Utc), 1, 1L, -200m },
                    { 8L, 3L, 10L, new DateTime(2025, 4, 28, 0, 0, 0, 0, DateTimeKind.Utc), 1, "Test transaction 1", null, "Amazon", "Movie tickets", new DateTime(2025, 4, 28, 0, 0, 0, 0, DateTimeKind.Utc), 1, 1L, -400m },
                    { 9L, 6L, 8L, new DateTime(2025, 4, 20, 0, 0, 0, 0, DateTimeKind.Utc), 0, "Test transaction 1", null, "Amazon", "Parents help", new DateTime(2025, 4, 20, 0, 0, 0, 0, DateTimeKind.Utc), 0, 1L, 600m },
                    { 10L, 2L, 11L, new DateTime(2025, 4, 25, 0, 0, 0, 0, DateTimeKind.Utc), 1, "Test transaction 1", null, "Amazon", "Food from market", new DateTime(2025, 4, 25, 0, 0, 0, 0, DateTimeKind.Utc), 1, 1L, -100m },
                    { 11L, 1L, 6L, new DateTime(2025, 4, 25, 0, 0, 0, 0, DateTimeKind.Utc), 1, "Test transaction 1", null, "Amazon", "Cafe breakfast", new DateTime(2025, 4, 25, 0, 0, 0, 0, DateTimeKind.Utc), 1, 1L, -200m },
                    { 12L, 5L, 5L, new DateTime(2025, 4, 4, 0, 0, 0, 0, DateTimeKind.Utc), 1, "Test transaction 1", null, "Amazon", "Some money for helping", new DateTime(2025, 4, 4, 0, 0, 0, 0, DateTimeKind.Utc), 0, 1L, 200m },
                    { 13L, 1L, 4L, new DateTime(2025, 4, 26, 0, 0, 0, 0, DateTimeKind.Utc), 1, "Test transaction 1", null, "Amazon", "Some new dishes", new DateTime(2025, 4, 26, 0, 0, 0, 0, DateTimeKind.Utc), 1, 1L, -300m },
                    { 14L, 4L, 3L, new DateTime(2025, 4, 28, 0, 0, 0, 0, DateTimeKind.Utc), 1, "Test transaction 1", null, "Amazon", "Salary again", new DateTime(2025, 4, 28, 0, 0, 0, 0, DateTimeKind.Utc), 0, 1L, 1100m },
                    { 15L, 4L, 12L, new DateTime(2025, 4, 29, 0, 0, 0, 0, DateTimeKind.Utc), 1, "Test transaction 1", null, "Amazon", "Flat rent", new DateTime(2025, 4, 29, 0, 0, 0, 0, DateTimeKind.Utc), 1, 1L, -1000m },
                    { 16L, 3L, 6L, new DateTime(2025, 4, 29, 0, 0, 0, 0, DateTimeKind.Utc), 1, "Test transaction 1", null, "Amazon", "Another day, another cafe", new DateTime(2025, 4, 29, 0, 0, 0, 0, DateTimeKind.Utc), 1, 1L, -300m }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                schema: "transactions",
                table: "Transactions",
                keyColumn: "Id",
                keyValue: 5L);

            migrationBuilder.DeleteData(
                schema: "transactions",
                table: "Transactions",
                keyColumn: "Id",
                keyValue: 6L);

            migrationBuilder.DeleteData(
                schema: "transactions",
                table: "Transactions",
                keyColumn: "Id",
                keyValue: 7L);

            migrationBuilder.DeleteData(
                schema: "transactions",
                table: "Transactions",
                keyColumn: "Id",
                keyValue: 8L);

            migrationBuilder.DeleteData(
                schema: "transactions",
                table: "Transactions",
                keyColumn: "Id",
                keyValue: 9L);

            migrationBuilder.DeleteData(
                schema: "transactions",
                table: "Transactions",
                keyColumn: "Id",
                keyValue: 10L);

            migrationBuilder.DeleteData(
                schema: "transactions",
                table: "Transactions",
                keyColumn: "Id",
                keyValue: 11L);

            migrationBuilder.DeleteData(
                schema: "transactions",
                table: "Transactions",
                keyColumn: "Id",
                keyValue: 12L);

            migrationBuilder.DeleteData(
                schema: "transactions",
                table: "Transactions",
                keyColumn: "Id",
                keyValue: 13L);

            migrationBuilder.DeleteData(
                schema: "transactions",
                table: "Transactions",
                keyColumn: "Id",
                keyValue: 14L);

            migrationBuilder.DeleteData(
                schema: "transactions",
                table: "Transactions",
                keyColumn: "Id",
                keyValue: 15L);

            migrationBuilder.DeleteData(
                schema: "transactions",
                table: "Transactions",
                keyColumn: "Id",
                keyValue: 16L);

            migrationBuilder.UpdateData(
                schema: "transactions",
                table: "Transactions",
                keyColumn: "Id",
                keyValue: 1L,
                columns: new[] { "CreationDate", "TransactionDate", "Value" },
                values: new object[] { new DateTime(2025, 2, 7, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2025, 2, 7, 0, 0, 0, 0, DateTimeKind.Utc), 100m });

            migrationBuilder.UpdateData(
                schema: "transactions",
                table: "Transactions",
                keyColumn: "Id",
                keyValue: 2L,
                columns: new[] { "CreationDate", "Title", "TransactionDate", "UserId" },
                values: new object[] { new DateTime(2025, 3, 1, 0, 0, 0, 0, DateTimeKind.Utc), "ABOBA for home", new DateTime(2025, 3, 1, 0, 0, 0, 0, DateTimeKind.Utc), 2L });

            migrationBuilder.UpdateData(
                schema: "transactions",
                table: "Transactions",
                keyColumn: "Id",
                keyValue: 3L,
                columns: new[] { "CreationDate", "TransactionDate", "UserId" },
                values: new object[] { new DateTime(2025, 2, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2025, 2, 1, 0, 0, 0, 0, DateTimeKind.Utc), 2L });

            migrationBuilder.UpdateData(
                schema: "transactions",
                table: "Transactions",
                keyColumn: "Id",
                keyValue: 4L,
                columns: new[] { "CreationDate", "TransactionDate", "UserId" },
                values: new object[] { new DateTime(2025, 2, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2025, 2, 1, 0, 0, 0, 0, DateTimeKind.Utc), 2L });
        }
    }
}
