using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CanaApp.Persistance.Data.Migrations
{
    /// <inheritdoc />
    public partial class EditInRecordAttributes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Records_Doctors_DoctorUserId",
                table: "Records");

            migrationBuilder.DropForeignKey(
                name: "FK_Records_Patients_PatientUserId",
                table: "Records");

            migrationBuilder.DropIndex(
                name: "IX_Records_DoctorUserId",
                table: "Records");

            migrationBuilder.DropColumn(
                name: "DoctorUserId",
                table: "Records");

            migrationBuilder.RenameColumn(
                name: "PatientUserId",
                table: "Records",
                newName: "UserId");

            migrationBuilder.RenameIndex(
                name: "IX_Records_PatientUserId",
                table: "Records",
                newName: "IX_Records_UserId");

            migrationBuilder.AddColumn<DateTime>(
                name: "Date",
                table: "Records",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "File",
                table: "Records",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Note",
                table: "Records",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: "18f2bb15-c5c3-4161-b3d5-ad8eab609da5",
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAEMt5smkE6xuDAoujQgwHWEleUxOYud3QDRhp0d7FtSHVifOi8xv8oGNo0K7bpHObpA==");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: "6dc6528a-b280-4770-9eae-82671ee81ef7",
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAEKV+jfb2e6Z2oITtTFWMTrXp0gtE+x22gIMNUJpaBimAHk8NG6KxhdwCENgzyDB4iw==");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: "8dbabc2f-4dd1-4ef6-acb4-7feb3fe5ed13",
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAEHZLyBPd+tuEq8KtM7X0AASP6XtuBPfBxlm7eVq3O5U/JJvrZt3FZicFEZVNLm1Rpw==");

            migrationBuilder.AddForeignKey(
                name: "FK_Records_Users_UserId",
                table: "Records",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Records_Users_UserId",
                table: "Records");

            migrationBuilder.DropColumn(
                name: "Date",
                table: "Records");

            migrationBuilder.DropColumn(
                name: "File",
                table: "Records");

            migrationBuilder.DropColumn(
                name: "Note",
                table: "Records");

            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "Records",
                newName: "PatientUserId");

            migrationBuilder.RenameIndex(
                name: "IX_Records_UserId",
                table: "Records",
                newName: "IX_Records_PatientUserId");

            migrationBuilder.AddColumn<string>(
                name: "DoctorUserId",
                table: "Records",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

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

            migrationBuilder.CreateIndex(
                name: "IX_Records_DoctorUserId",
                table: "Records",
                column: "DoctorUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Records_Doctors_DoctorUserId",
                table: "Records",
                column: "DoctorUserId",
                principalTable: "Doctors",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Records_Patients_PatientUserId",
                table: "Records",
                column: "PatientUserId",
                principalTable: "Patients",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
