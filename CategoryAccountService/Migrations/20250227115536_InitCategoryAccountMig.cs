using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace CategoryAccountService.Migrations
{
    /// <inheritdoc />
    public partial class InitCategoryAccountMig : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "category_accounts");

            migrationBuilder.CreateTable(
                name: "Accounts",
                schema: "category_accounts",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    UserId = table.Column<long>(type: "bigint", nullable: false),
                    AccountName = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: false),
                    AccountType = table.Column<int>(type: "integer", nullable: false),
                    Currency = table.Column<int>(type: "integer", nullable: false),
                    Balance = table.Column<int>(type: "integer", nullable: false),
                    Description = table.Column<string>(type: "character varying(512)", maxLength: 512, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Accounts", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Categories",
                schema: "category_accounts",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    UserId = table.Column<long>(type: "bigint", nullable: true),
                    CategoryName = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: false),
                    CategoryType = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Categories", x => x.Id);
                });

            migrationBuilder.InsertData(
                schema: "category_accounts",
                table: "Accounts",
                columns: new[] { "Id", "AccountName", "AccountType", "Balance", "Currency", "Description", "UserId" },
                values: new object[,]
                {
                    { 1L, "Checking Account", 0, 1500, 1, "Primary checking account", 1L },
                    { 2L, "Savings Account", 1, 5000, 0, "Long-term savings account", 2L }
                });

            migrationBuilder.InsertData(
                schema: "category_accounts",
                table: "Categories",
                columns: new[] { "Id", "CategoryName", "CategoryType", "UserId" },
                values: new object[,]
                {
                    { 1L, "Groceries", 1, 1L },
                    { 2L, "Salary", 0, 2L }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Accounts",
                schema: "category_accounts");

            migrationBuilder.DropTable(
                name: "Categories",
                schema: "category_accounts");
        }
    }
}
