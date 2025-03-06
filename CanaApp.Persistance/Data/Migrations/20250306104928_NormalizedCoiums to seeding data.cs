using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace CanaApp.Persistance.Data.Migrations
{
    /// <inheritdoc />
    public partial class NormalizedCoiumstoseedingdata : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: "19bdd948-3698-48aa-88cd-1e3098bad4d5");

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: "28fc2e6d-6d5e-4258-aef9-220f23e692a7");

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: "ef369b71-68af-4fd9-9ef6-c6396867e0d3");

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "AccessFailedCount", "Address", "ConcurrencyStamp", "Email", "EmailConfirmed", "Image", "LockoutEnabled", "LockoutEnd", "Name", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "TwoFactorEnabled", "UserName", "UserType" },
                values: new object[,]
                {
                    { "60ccd7ad-0ad8-4093-856d-9e0e5bbc8517", 0, "Egypt, fayoum", "99d2bbc6-bc54-4248-a172-a77de3ae4430", "admin@Canc-app.com", true, null, false, null, "Mohamed Yasser", "ADMIN@CANC-APP.COM", "MOHAMED_YASSER", "AQAAAAIAAYagAAAAECFKkvOi8pCyYpeePUxlKPxB8FfOpSLDcbafh+7iM5DfRqHPagT+hu0y/jtOdZ9huA==", null, false, "55BF92C9EF0249CDA210D85D1A851BC9", false, "Mohamed_Yasser", "Admin" },
                    { "cd1aaf29-6373-4509-91cd-18631b772e41", 0, "Egypt, fayoum", "d0831003-9587-495a-8e7d-d350e009366e", "Patient@Canc-app.com", true, null, false, null, "Mohamed Soliman", "PATIENT@CANC-APP.COM", "MOHAMED_SOLIMAN", "AQAAAAIAAYagAAAAEN3BUx8EVOHkOLzwtn6c3h6ZkuG2nutD7hs8x98xxOzm2hq/IVu+eREQQz0NLGG03A==", null, false, "b9d50bbf5e87434fa03edc01413ca904", false, "Mohamed_Soliman", "Patient" },
                    { "fdcea14c-5c46-41fc-b3cd-c305d46b3984", 0, "Egypt, fayoum", "c97edf97-9d17-41f0-ae2c-04ff10d11c19", "Doctor@Canc-app.com", true, null, false, null, "Zyad Mahmoud", "DOCTOR@CANC-APP.COM", "ZYAD_MAHMOUD", "AQAAAAIAAYagAAAAEAPD9yWLFqG+rsY6H+y7pPgWOpdEOiDv6sUlb/MspUrHRgarSRoaGxwob6Hzuq4awg==", null, false, "85a5af679fe14a24a17a7b9af212c4dc", false, "Zyad_Mahmoud", "Doctor" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: "60ccd7ad-0ad8-4093-856d-9e0e5bbc8517");

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: "cd1aaf29-6373-4509-91cd-18631b772e41");

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: "fdcea14c-5c46-41fc-b3cd-c305d46b3984");

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "AccessFailedCount", "Address", "ConcurrencyStamp", "Email", "EmailConfirmed", "Image", "LockoutEnabled", "LockoutEnd", "Name", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "TwoFactorEnabled", "UserName", "UserType" },
                values: new object[,]
                {
                    { "19bdd948-3698-48aa-88cd-1e3098bad4d5", 0, "Egypt, fayoum", "c97edf97-9d17-41f0-ae2c-04ff10d11c19", "Doctor@Canc-app.com", true, null, false, null, "Zyad Mahmoud", null, null, "AQAAAAIAAYagAAAAEDDIvsr6tSYc4DXOpGVJpuOAh2z/Odt2N+P5COxBIDoo9X95n7db/DPX/DQBSb+eYw==", null, false, "85a5af679fe14a24a17a7b9af212c4dc", false, "Zyad_Mahmoud", "Doctor" },
                    { "28fc2e6d-6d5e-4258-aef9-220f23e692a7", 0, "Egypt, fayoum", "d0831003-9587-495a-8e7d-d350e009366e", "Patient@Canc-app.com", true, null, false, null, "Mohamed Soliman", null, null, "AQAAAAIAAYagAAAAEFux8+OCO7fpaCB0pXw4fzDOy9VVLHwoBGHjmfdVxHXizxzzQSff7lfmaiEe74PTRQ==", null, false, "b9d50bbf5e87434fa03edc01413ca904", false, "Mohamed_Soliman", "Patient" },
                    { "ef369b71-68af-4fd9-9ef6-c6396867e0d3", 0, "Egypt, fayoum", "99d2bbc6-bc54-4248-a172-a77de3ae4430", "admin@Canc-app.com", true, null, false, null, "Mohamed Yasser", null, null, "AQAAAAIAAYagAAAAEDpucXi5Bnp9fnY37umXxAq1StBZaOGK8rpj2I52QDhcUMnzlBs6YbJkEMsor+Pe1w==", null, false, "55BF92C9EF0249CDA210D85D1A851BC9", false, "Mohamed_Yasser", "Admin" }
                });
        }
    }
}
