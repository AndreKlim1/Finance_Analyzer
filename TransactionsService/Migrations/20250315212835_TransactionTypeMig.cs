using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TransactionsService.Migrations
{
    /// <inheritdoc />
    public partial class TransactionTypeMig : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "PaymentMethod",
                schema: "transactions",
                table: "Transactions",
                newName: "TransactionType");

            migrationBuilder.AlterColumn<long>(
                name: "AccountId",
                schema: "transactions",
                table: "Transactions",
                type: "bigint",
                nullable: false,
                defaultValue: 0L,
                oldClrType: typeof(long),
                oldType: "bigint",
                oldNullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "TransactionType",
                schema: "transactions",
                table: "Transactions",
                newName: "PaymentMethod");

            migrationBuilder.AlterColumn<long>(
                name: "AccountId",
                schema: "transactions",
                table: "Transactions",
                type: "bigint",
                nullable: true,
                oldClrType: typeof(long),
                oldType: "bigint");
        }
    }
}
