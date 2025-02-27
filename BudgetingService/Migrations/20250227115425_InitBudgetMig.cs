using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace BudgetingService.Migrations
{
    /// <inheritdoc />
    public partial class InitBudgetMig : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "budgets");

            migrationBuilder.CreateTable(
                name: "Budgets",
                schema: "budgets",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    UserId = table.Column<long>(type: "bigint", nullable: false),
                    CategoryId = table.Column<long>(type: "bigint", nullable: true),
                    BudgetName = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: false),
                    Description = table.Column<string>(type: "character varying(512)", maxLength: 512, nullable: false),
                    PlannedAmount = table.Column<int>(type: "integer", nullable: false),
                    Currency = table.Column<int>(type: "integer", nullable: false),
                    PeriodStart = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    PeriodEnd = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    BudgetStatus = table.Column<int>(type: "integer", nullable: false),
                    BudgetType = table.Column<int>(type: "integer", nullable: false),
                    WarningThreshold = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Budgets", x => x.Id);
                });

            migrationBuilder.InsertData(
                schema: "budgets",
                table: "Budgets",
                columns: new[] { "Id", "BudgetName", "BudgetStatus", "BudgetType", "CategoryId", "Currency", "Description", "PeriodEnd", "PeriodStart", "PlannedAmount", "UserId", "WarningThreshold" },
                values: new object[,]
                {
                    { 1L, "Monthly Groceries", 0, 1, 1L, 1, "Budget for monthly grocery shopping", new DateTime(2025, 3, 31, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2025, 3, 1, 0, 0, 0, 0, DateTimeKind.Utc), 500, 1L, 80 },
                    { 2L, "Vacation Savings", 2, 0, 2L, 0, "Saving up for a summer vacation", new DateTime(2025, 12, 31, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 2000, 2L, 90 }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Budgets",
                schema: "budgets");
        }
    }
}
