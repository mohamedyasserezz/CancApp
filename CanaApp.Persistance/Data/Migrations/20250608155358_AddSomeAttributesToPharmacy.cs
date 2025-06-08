using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CanaApp.Persistance.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddSomeAttributesToPharmacy : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "NumberOfWorkingHours",
                table: "Pharmacists");

            migrationBuilder.AddColumn<TimeOnly>(
                name: "CloseHour",
                table: "Pharmacists",
                type: "time",
                nullable: false,
                defaultValue: new TimeOnly(0, 0, 0));

            migrationBuilder.AddColumn<float>(
                name: "Latitude",
                table: "Pharmacists",
                type: "real",
                nullable: false,
                defaultValue: 0f);

            migrationBuilder.AddColumn<float>(
                name: "Longitude",
                table: "Pharmacists",
                type: "real",
                nullable: false,
                defaultValue: 0f);

            migrationBuilder.AddColumn<TimeOnly>(
                name: "OpeningHour",
                table: "Pharmacists",
                type: "time",
                nullable: false,
                defaultValue: new TimeOnly(0, 0, 0));

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: "18f2bb15-c5c3-4161-b3d5-ad8eab609da5",
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAECw67jqi8lvghjANJfdc9U69zIoIC2sgaSG6ZpwMoC+zR5L4hIxloyixQFu01dR84Q==");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: "6dc6528a-b280-4770-9eae-82671ee81ef7",
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAEKceeArT+h0rPxCmpphqhsfQGkcDo7PD7Hb3y3q1rpgxQ7BBEdwW4sYiBepuTICfxg==");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: "8dbabc2f-4dd1-4ef6-acb4-7feb3fe5ed13",
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAEDHeMb5MiNpYJj0jWDByV/q687XlZt1KqhC2ON3yikm9UGu+EUckYYw6075iS60jsg==");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CloseHour",
                table: "Pharmacists");

            migrationBuilder.DropColumn(
                name: "Latitude",
                table: "Pharmacists");

            migrationBuilder.DropColumn(
                name: "Longitude",
                table: "Pharmacists");

            migrationBuilder.DropColumn(
                name: "OpeningHour",
                table: "Pharmacists");

            migrationBuilder.AddColumn<int>(
                name: "NumberOfWorkingHours",
                table: "Pharmacists",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: "18f2bb15-c5c3-4161-b3d5-ad8eab609da5",
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAELN4ZrDxUrJwOoRHtEoFuXIVpnI6rz9O/NhTzL/iXof9VMMYAFj1MfSgPoKPac8bww==");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: "6dc6528a-b280-4770-9eae-82671ee81ef7",
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAEGqzomqmfl7edw73xHbCpGfO+OIX1s4Cz2+pPjuEqGLx4OR/qoCo/swpgyEMQp+Txg==");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: "8dbabc2f-4dd1-4ef6-acb4-7feb3fe5ed13",
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAEDrVjpkDsvPPsP7Ro5YDzK2ZC0JyhVgB8lZWlWUKQBb9ww8gUGkn+jlB/eFPDPmS1w==");
        }
    }
}
