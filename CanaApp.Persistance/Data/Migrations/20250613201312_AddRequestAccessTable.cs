using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CanaApp.Persistance.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddRequestAccessTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "RecordAccessRequests",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DoctorId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    PatientId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    RequestedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsApproved = table.Column<bool>(type: "bit", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RecordAccessRequests", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RecordAccessRequests_Users_DoctorId",
                        column: x => x.DoctorId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_RecordAccessRequests_Users_PatientId",
                        column: x => x.PatientId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: "18f2bb15-c5c3-4161-b3d5-ad8eab609da5",
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAEClqc6hfzmjh4+F97EjcRYkFurGVyRhFaYP3C3nc3SzfgaQcKGhso6nO3vxhYDoW2Q==");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: "6dc6528a-b280-4770-9eae-82671ee81ef7",
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAEMdKnoqDrb5+TjxYuaJ37xEtgRWR1HSXZezJiiBjgg8lDNS9K6xPtDWqVDr/PzwcYw==");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: "8dbabc2f-4dd1-4ef6-acb4-7feb3fe5ed13",
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAEDit26wpgs2yVwbOw8QWwV5HibCQumofMM+PgIizjrL9nsz/lkWvflyDHcy4gMP32A==");

            migrationBuilder.CreateIndex(
                name: "IX_RecordAccessRequests_DoctorId",
                table: "RecordAccessRequests",
                column: "DoctorId");

            migrationBuilder.CreateIndex(
                name: "IX_RecordAccessRequests_PatientId",
                table: "RecordAccessRequests",
                column: "PatientId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "RecordAccessRequests");

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
        }
    }
}
