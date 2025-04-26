using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CanaApp.Persistance.Data.Migrations
{
    /// <inheritdoc />
    public partial class IsForgetPasswordOtpConfirmed : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsForgetPasswordOtpConfirmed",
                table: "Users",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: "18f2bb15-c5c3-4161-b3d5-ad8eab609da5",
                columns: new[] { "IsForgetPasswordOtpConfirmed", "PasswordHash" },
                values: new object[] { false, "AQAAAAIAAYagAAAAEAehWUWWZy4LgE/J3gF6mfOaK+7AyNfJnKvcL1dsTgyzp7b1zJF5ZgSBUO+R4De6VQ==" });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: "6dc6528a-b280-4770-9eae-82671ee81ef7",
                columns: new[] { "IsForgetPasswordOtpConfirmed", "PasswordHash" },
                values: new object[] { false, "AQAAAAIAAYagAAAAEOaCpUZfPkRM1tkPVW+0Kk4Hz/m65jMlb9NNJcru929pR2bzSTpaZbMrYFo3PNNDfw==" });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: "8dbabc2f-4dd1-4ef6-acb4-7feb3fe5ed13",
                columns: new[] { "IsForgetPasswordOtpConfirmed", "PasswordHash" },
                values: new object[] { false, "AQAAAAIAAYagAAAAEBI/PZSjI/fqyoXMoQUACLMVXiU33aJWyCeUkvC95zrPy/PBIbzZaanncUcVt1voiA==" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsForgetPasswordOtpConfirmed",
                table: "Users");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: "18f2bb15-c5c3-4161-b3d5-ad8eab609da5",
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAECWaa7+EqLb7WKwCZOGYlArK3zqo/HH5cfUQJXp24I/f0tKBPr8zbzEDlt79Y87AWA==");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: "6dc6528a-b280-4770-9eae-82671ee81ef7",
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAEFbFw/452OzenmcvCxISgAKkW0/YdfPBadoXGHUDKD6o5pCr0D1wAypsMW0xWvo5og==");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: "8dbabc2f-4dd1-4ef6-acb4-7feb3fe5ed13",
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAECnFt48/pW4S7zjKeeb6oIowVe+Y4aaKkyUbYL6VVI5drnARLnccmkvrkbitFR1i/g==");
        }
    }
}
