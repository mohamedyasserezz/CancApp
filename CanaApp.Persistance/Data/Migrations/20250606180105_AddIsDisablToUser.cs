using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CanaApp.Persistance.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddIsDisablToUser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsDisabled",
                table: "Volunteers");

            migrationBuilder.DropColumn(
                name: "NumberOfWarrings",
                table: "Volunteers");

            migrationBuilder.DropColumn(
                name: "IsDisabled",
                table: "Psychiatrists");

            migrationBuilder.DropColumn(
                name: "NumberOfWarrings",
                table: "Psychiatrists");

            migrationBuilder.DropColumn(
                name: "IsDisabled",
                table: "Pharmacists");

            migrationBuilder.DropColumn(
                name: "NumberOfWarrings",
                table: "Pharmacists");

            migrationBuilder.DropColumn(
                name: "IsDisabled",
                table: "Patients");

            migrationBuilder.DropColumn(
                name: "NumberOfWarrings",
                table: "Patients");

            migrationBuilder.DropColumn(
                name: "IsDisabled",
                table: "Doctors");

            migrationBuilder.DropColumn(
                name: "NumberOfWarrings",
                table: "Doctors");

            migrationBuilder.RenameColumn(
                name: "IsDisaired",
                table: "Users",
                newName: "IsDisabled");

            migrationBuilder.AddColumn<int>(
                name: "NumberOfWarrings",
                table: "Users",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: "18f2bb15-c5c3-4161-b3d5-ad8eab609da5",
                columns: new[] { "NumberOfWarrings", "PasswordHash" },
                values: new object[] { 0, "AQAAAAIAAYagAAAAED4Mgi/Qlrvjg0Wk3gAmhqXGLKRqgRjlICPRygvxBUvlKw66mjMsPbI5uH17xXEkxg==" });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: "6dc6528a-b280-4770-9eae-82671ee81ef7",
                columns: new[] { "NumberOfWarrings", "PasswordHash" },
                values: new object[] { 0, "AQAAAAIAAYagAAAAEO6UHNwjrHplYv/4AOKXaz4XLy8PHTUsGCyCBwoqPc2YgAYhYRjuc/aillUY1VkdeQ==" });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: "8dbabc2f-4dd1-4ef6-acb4-7feb3fe5ed13",
                columns: new[] { "NumberOfWarrings", "PasswordHash" },
                values: new object[] { 0, "AQAAAAIAAYagAAAAEJiZ4KRQSNr6Ke+kq0/bNc2wNkQk2u0o/WFZtS7ZfjtVnjvw5j1o0fpG/XQL5uJhLw==" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "NumberOfWarrings",
                table: "Users");

            migrationBuilder.RenameColumn(
                name: "IsDisabled",
                table: "Users",
                newName: "IsDisaired");

            migrationBuilder.AddColumn<bool>(
                name: "IsDisabled",
                table: "Volunteers",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "NumberOfWarrings",
                table: "Volunteers",
                type: "int",
                nullable: false,
                defaultValue: 0);

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
                name: "IsDisabled",
                table: "Pharmacists",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "NumberOfWarrings",
                table: "Pharmacists",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<bool>(
                name: "IsDisabled",
                table: "Patients",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "NumberOfWarrings",
                table: "Patients",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<bool>(
                name: "IsDisabled",
                table: "Doctors",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "NumberOfWarrings",
                table: "Doctors",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.UpdateData(
                table: "Doctors",
                keyColumn: "UserId",
                keyValue: "8dbabc2f-4dd1-4ef6-acb4-7feb3fe5ed13",
                columns: new[] { "IsDisabled", "NumberOfWarrings" },
                values: new object[] { false, 0 });

            migrationBuilder.UpdateData(
                table: "Patients",
                keyColumn: "UserId",
                keyValue: "18f2bb15-c5c3-4161-b3d5-ad8eab609da5",
                columns: new[] { "IsDisabled", "NumberOfWarrings" },
                values: new object[] { false, 0 });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: "18f2bb15-c5c3-4161-b3d5-ad8eab609da5",
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAEFo55a6l7CvKcmEEgARTCHDmpvaGbw59oc0o2BXBqivOR7lM9D8HlwMRfJOqYxmKsw==");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: "6dc6528a-b280-4770-9eae-82671ee81ef7",
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAEF0nMqtlrJ93lO2+Zn4XMqFOq06xAppJHI2NkZobxgrVf5RRud3TMTknEA2UAqP+LA==");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: "8dbabc2f-4dd1-4ef6-acb4-7feb3fe5ed13",
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAEKgMO1cd3c3wreDaoFprufUvqMVxEZcPdLcD5Y1T4F+SVyvOpCpgfvLIZ6sElE+E7A==");
        }
    }
}
