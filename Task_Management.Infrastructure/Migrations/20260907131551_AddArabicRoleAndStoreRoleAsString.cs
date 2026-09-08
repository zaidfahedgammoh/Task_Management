using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Task_Management.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddArabicRoleAndStoreRoleAsString : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "role",
                table: "Users",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int"
                );
                migrationBuilder.Sql("""
    UPDATE Users
    SET role =
        CASE role
            WHEN '0' THEN 'TTL'
            WHEN '1' THEN 'Employee'
            WHEN '2' THEN 'Manager'
        END;
    """);

            migrationBuilder.AddColumn<string>(
                name: "role_ar",
                table: "Users",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "role_ar",
                table: "Users");

            migrationBuilder.AlterColumn<int>(
                name: "role",
                table: "Users",
                type: "int",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");
        }
    }
}
