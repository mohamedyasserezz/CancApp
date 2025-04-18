using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CanaApp.Persistance.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddPatient : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Patients",
                columns: new[] { "UserId", "IsDisabled", "NumberOfWarrings" },
                values: new object[] { "18f2bb15-c5c3-4161-b3d5-ad8eab609da5", false, 0 });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: "18f2bb15-c5c3-4161-b3d5-ad8eab609da5",
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAEHmkNEEXv5Sc0qhujBHReFlhZ5HoUVePV+gCLAK9ucPeIzlGDfr6Rg75Sx8tFK6Rug==");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: "6dc6528a-b280-4770-9eae-82671ee81ef7",
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAEDo9hXMT61phA8Hl1Q2dIu7Wi9iEF9illDm3dZuZnrd6J5uDFhczGAz0zN+eDMw+ZQ==");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: "8dbabc2f-4dd1-4ef6-acb4-7feb3fe5ed13",
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAEGAwDnP7kXzBluvHc880B9y9TScafTohuqtZRzD/r56y4kuybYLJKlvMoPiNfJ+NBg==");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Patients",
                keyColumn: "UserId",
                keyValue: "18f2bb15-c5c3-4161-b3d5-ad8eab609da5");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: "18f2bb15-c5c3-4161-b3d5-ad8eab609da5",
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAEGXLUxxNXDwkPVMLood7ooGZJ4SrSog7kS3IqFY9CoMShbGkGcIqhjqE/EdkE4uExw==");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: "6dc6528a-b280-4770-9eae-82671ee81ef7",
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAEDkznKOktVq/ToFxBUvXQxj8+BLQNu3cIcUsHfAIStFEPEpvEp9Gqf9FpEO+TnZCxg==");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: "8dbabc2f-4dd1-4ef6-acb4-7feb3fe5ed13",
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAEM/K0gpxHIqHc2L2eMog+t9pqvSBF468pobCERtq+BF66iYCKvXMd7v2ZZB1BQfrkQ==");
        }
    }
}
