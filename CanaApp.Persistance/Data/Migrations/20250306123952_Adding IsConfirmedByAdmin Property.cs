using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CanaApp.Persistance.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddingIsConfirmedByAdminProperty : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsConfirmedByAdmin",
                table: "Psychiatrists",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsConfirmedByAdmin",
                table: "Pharmacists",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsConfirmedByAdmin",
                table: "Doctors",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: "18f2bb15-c5c3-4161-b3d5-ad8eab609da5",
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAEHpZJPSCvMXFiO8Yq/zMgQcO/eEGrRtggdpnjBRBmgWyuirlqA2IYNem2UECXMXyOA==");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: "6dc6528a-b280-4770-9eae-82671ee81ef7",
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAEHxgLuO7bvX59AgL+u57O68AJAE7sC6BeZP/l3gewYFsC0FsS5zEFaMjaDhg87I87Q==");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: "8dbabc2f-4dd1-4ef6-acb4-7feb3fe5ed13",
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAEOZ05BB7qO2GGLU4I+Hn1B2YHfhQGZYY9HEP5MgeObtXuJTk8ulhu39tIs20txpotQ==");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsConfirmedByAdmin",
                table: "Psychiatrists");

            migrationBuilder.DropColumn(
                name: "IsConfirmedByAdmin",
                table: "Pharmacists");

            migrationBuilder.DropColumn(
                name: "IsConfirmedByAdmin",
                table: "Doctors");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: "18f2bb15-c5c3-4161-b3d5-ad8eab609da5",
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAEHE0DBT0NjWYFhPq9YZ+vvqMxtnnu6TJhDPJkR+b4kmd2XsQVzGNsVyXKnec9tlaDw==");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: "6dc6528a-b280-4770-9eae-82671ee81ef7",
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAENb1rFSTXNwqK3luss66RiCZB1PZxAEXzpnh+mFV/clgdokmnyEzi8gmZ2fLp7r7Iw==");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: "8dbabc2f-4dd1-4ef6-acb4-7feb3fe5ed13",
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAEGD8LFyILYpwMSqmzV4KpUy5K8KvLUym/NX3IPFNAWoyzqdWTlxZXVWGdVoTuk+zlg==");
        }
    }
}
