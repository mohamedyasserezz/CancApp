using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CanaApp.Persistance.Data.Migrations
{
    /// <inheritdoc />
    public partial class Changesomeattributes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsCompletedProfileFailed",
                table: "Psychiatrists",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsDisabled",
                table: "Psychiatrists",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "NumberOfWarrings",
                table: "Psychiatrists",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<bool>(
                name: "IsCompletedProfileFailed",
                table: "Pharmacists",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsCompletedProfileFailed",
                table: "Doctors",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: "18f2bb15-c5c3-4161-b3d5-ad8eab609da5",
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAEJ/jzgvKiN1eS5+S1Ps8I/0sgyVv1x3rzo+UpLbO1YDtOk/2M7eylY+qY6IK+NXosA==");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: "6dc6528a-b280-4770-9eae-82671ee81ef7",
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAEBrrh6LckRd7F52NEgP+/+gG00nKR5ZL2P1mhYnWRf8hziLfLZO2H2eWNei5gWuA2A==");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: "8dbabc2f-4dd1-4ef6-acb4-7feb3fe5ed13",
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAEIVjnbJiZDZp4UHZ+AixAsungt2BEIF+FF/OqdwWqfhV8aCPKmkw0OXUtW3Bj6dpOw==");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsCompletedProfileFailed",
                table: "Psychiatrists");

            migrationBuilder.DropColumn(
                name: "IsDisabled",
                table: "Psychiatrists");

            migrationBuilder.DropColumn(
                name: "NumberOfWarrings",
                table: "Psychiatrists");

            migrationBuilder.DropColumn(
                name: "IsCompletedProfileFailed",
                table: "Pharmacists");

            migrationBuilder.DropColumn(
                name: "IsCompletedProfileFailed",
                table: "Doctors");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: "18f2bb15-c5c3-4161-b3d5-ad8eab609da5",
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAEKQnFlWJtmBpbhDYlTy1MG/wUizMXEkug3YrXYkPc3ZS7kQUdJ7/djUnGPzemBzjpw==");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: "6dc6528a-b280-4770-9eae-82671ee81ef7",
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAEJSmKT7GfcCWMJXQ9hI+3cazf4PM/5wW/SGZ0mTZRdPRGU/0rKlVJuwddbW09YvLyg==");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: "8dbabc2f-4dd1-4ef6-acb4-7feb3fe5ed13",
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAEGFTU0fK0kZeVjV4upGvDRK3bwKPhvMi8dw13x5F/jjVPpHaBa73dsbfWbK8zPny1g==");
        }
    }
}
