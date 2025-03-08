using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CanaApp.Persistance.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddHangfire : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Address",
                table: "Users",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(200)",
                oldMaxLength: 200);

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: "18f2bb15-c5c3-4161-b3d5-ad8eab609da5",
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAEEoSdn+hW2ioLvcJA4SVCkO0zniro25LRnPAjcC8yHkFrMOzFYbq4oAbbfvlzLUCRg==");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: "6dc6528a-b280-4770-9eae-82671ee81ef7",
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAEJhCqL+Y3zJ9Gx3thpcw8H6ZvrvlBBjixKPCEIvGwFfLhuKEqclZwPabQhnR34qUcw==");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: "8dbabc2f-4dd1-4ef6-acb4-7feb3fe5ed13",
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAELp7aup1TNr+B/CEKAzOxmMjHiQwBTprQeg1JX1EJgZj7gmvs5vor7TCr5i0Ni/vBQ==");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Address",
                table: "Users",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(200)",
                oldMaxLength: 200,
                oldNullable: true);

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: "18f2bb15-c5c3-4161-b3d5-ad8eab609da5",
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAEHFaghKED6ifI5lYc0ZxSAIg2ZZ1z44uHDtE3GRe+j6OSi/w6wnKdjdOVIczJ+Owgg==");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: "6dc6528a-b280-4770-9eae-82671ee81ef7",
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAEJ64SwNzf5we5164PHpXPTK6ffauIffo5hSM19Ex8VMnuU7+Dp0I5JwntAPV0Xf22Q==");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: "8dbabc2f-4dd1-4ef6-acb4-7feb3fe5ed13",
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAEHV8BOQJCW1PI47REH0VyoiGYFCvEeObpsIQ8H4R4ItxBmIGxPKGl0H0emUSo1TnSg==");
        }
    }
}
