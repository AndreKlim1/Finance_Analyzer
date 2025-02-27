using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace TransactionsService.Migrations
{
    /// <inheritdoc />
    public partial class InitTransactionMig : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "transactions");

            migrationBuilder.CreateTable(
                name: "Transactions",
                schema: "transactions",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Value = table.Column<int>(type: "integer", nullable: false),
                    Currency = table.Column<int>(type: "integer", nullable: false),
                    CategoryId = table.Column<long>(type: "bigint", nullable: false),
                    AccountId = table.Column<long>(type: "bigint", nullable: true),
                    UserId = table.Column<long>(type: "bigint", nullable: false),
                    Description = table.Column<string>(type: "character varying(512)", maxLength: 512, nullable: true),
                    Image = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    TransactionDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreationDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    PaymentMethod = table.Column<int>(type: "integer", nullable: false),
                    Merchant = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Transactions", x => x.Id);
                });

            migrationBuilder.InsertData(
                schema: "transactions",
                table: "Transactions",
                columns: new[] { "Id", "AccountId", "CategoryId", "CreationDate", "Currency", "Description", "Image", "Merchant", "PaymentMethod", "TransactionDate", "UserId", "Value" },
                values: new object[,]
                {
                    { 1L, 1L, 1L, new DateTime(2025, 2, 1, 0, 0, 0, 0, DateTimeKind.Utc), 1, "Test transaction 1", null, "Amazon", 0, new DateTime(2025, 2, 1, 0, 0, 0, 0, DateTimeKind.Utc), 1L, 100 },
                    { 2L, 2L, 2L, new DateTime(2025, 2, 1, 0, 0, 0, 0, DateTimeKind.Utc), 0, "Test transaction 2", null, "Local Store", 1, new DateTime(2025, 2, 1, 0, 0, 0, 0, DateTimeKind.Utc), 2L, 200 }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Transactions",
                schema: "transactions");
        }
    }
}
