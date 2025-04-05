using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace TransactionsService.Migrations
{
    /// <inheritdoc />
    public partial class EnhancedDataset : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                schema: "transactions",
                table: "Transactions",
                keyColumn: "Id",
                keyValue: 1L,
                columns: new[] { "CategoryId", "CreationDate", "TransactionDate" },
                values: new object[] { 3L, new DateTime(2025, 2, 7, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2025, 2, 7, 0, 0, 0, 0, DateTimeKind.Utc) });

            migrationBuilder.UpdateData(
                schema: "transactions",
                table: "Transactions",
                keyColumn: "Id",
                keyValue: 2L,
                columns: new[] { "CategoryId", "CreationDate", "Title", "TransactionDate", "Value" },
                values: new object[] { 4L, new DateTime(2025, 3, 1, 0, 0, 0, 0, DateTimeKind.Utc), "ABOBA for home", new DateTime(2025, 3, 1, 0, 0, 0, 0, DateTimeKind.Utc), -200 });

            migrationBuilder.InsertData(
                schema: "transactions",
                table: "Transactions",
                columns: new[] { "Id", "AccountId", "CategoryId", "CreationDate", "Currency", "Description", "Image", "Merchant", "Title", "TransactionDate", "TransactionType", "UserId", "Value" },
                values: new object[,]
                {
                    { 3L, 2L, 6L, new DateTime(2025, 2, 1, 0, 0, 0, 0, DateTimeKind.Utc), 0, "Test transaction 2", null, "Local Store", "Cheeeeese", new DateTime(2025, 2, 1, 0, 0, 0, 0, DateTimeKind.Utc), 1, 2L, -400 },
                    { 4L, 2L, 5L, new DateTime(2025, 2, 1, 0, 0, 0, 0, DateTimeKind.Utc), 1, "Test transaction 2", null, "Local Store", "Coins from fountain", new DateTime(2025, 2, 1, 0, 0, 0, 0, DateTimeKind.Utc), 0, 2L, 100 }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                schema: "transactions",
                table: "Transactions",
                keyColumn: "Id",
                keyValue: 3L);

            migrationBuilder.DeleteData(
                schema: "transactions",
                table: "Transactions",
                keyColumn: "Id",
                keyValue: 4L);

            migrationBuilder.UpdateData(
                schema: "transactions",
                table: "Transactions",
                keyColumn: "Id",
                keyValue: 1L,
                columns: new[] { "CategoryId", "CreationDate", "TransactionDate" },
                values: new object[] { 1L, new DateTime(2025, 2, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2025, 2, 1, 0, 0, 0, 0, DateTimeKind.Utc) });

            migrationBuilder.UpdateData(
                schema: "transactions",
                table: "Transactions",
                keyColumn: "Id",
                keyValue: 2L,
                columns: new[] { "CategoryId", "CreationDate", "Title", "TransactionDate", "Value" },
                values: new object[] { 2L, new DateTime(2025, 2, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Second transaction", new DateTime(2025, 2, 1, 0, 0, 0, 0, DateTimeKind.Utc), 200 });
        }
    }
}
