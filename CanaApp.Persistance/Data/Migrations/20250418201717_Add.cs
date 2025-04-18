using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CanaApp.Persistance.Data.Migrations
{
    /// <inheritdoc />
    public partial class Add : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
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

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "AccessFailedCount", "Address", "ConcurrencyStamp", "Email", "EmailConfirmed", "FullName", "Image", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "TwoFactorEnabled", "UserName", "UserType" },
                values: new object[] { "18f2bb15-c5c3-4161-b3d5-ad8eab609da5", 0, "Egypt, fayoum", "d0831003-9587-495a-8e7d-d350e009366e", "Patient@Canc-app.com", true, "Mohamed Soliman", null, false, null, "PATIENT@CANC-APP.COM", "MOHAMED_SOLIMAN", "AQAAAAIAAYagAAAAEGXLUxxNXDwkPVMLood7ooGZJ4SrSog7kS3IqFY9CoMShbGkGcIqhjqE/EdkE4uExw==", null, false, "b9d50bbf5e87434fa03edc01413ca904", false, "Mohamed_Soliman", "Patient" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: "18f2bb15-c5c3-4161-b3d5-ad8eab609da5");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: "6dc6528a-b280-4770-9eae-82671ee81ef7",
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAEEuwpo1LN6LSkmgE3NW4l0QPY1/w1t9jXzb/Y63qn2mTMCoBgji0YzFbeN6N36AmQw==");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: "8dbabc2f-4dd1-4ef6-acb4-7feb3fe5ed13",
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAEPkqJRWiddZhE+5ibdPflFy+APRw2HUukdnpWCOAQ2Xpyc4wEnf8TB5wFOTshN0XyQ==");
        }
    }
}
