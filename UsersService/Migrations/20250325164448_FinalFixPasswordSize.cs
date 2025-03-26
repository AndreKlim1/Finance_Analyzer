using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace UsersService.Migrations
{
    /// <inheritdoc />
    public partial class FinalFixPasswordSize : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "PasswordHash",
                schema: "users",
                table: "Users",
                type: "character(60)",
                fixedLength: true,
                maxLength: 60,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character(256)",
                oldFixedLength: true,
                oldMaxLength: 256);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "PasswordHash",
                schema: "users",
                table: "Users",
                type: "character(256)",
                fixedLength: true,
                maxLength: 256,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character(60)",
                oldFixedLength: true,
                oldMaxLength: 60);
        }
    }
}
