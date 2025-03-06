using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace CanaApp.Persistance.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddIdToSeedingData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
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
                    { "18f2bb15-c5c3-4161-b3d5-ad8eab609da5", 0, "Egypt, fayoum", "d0831003-9587-495a-8e7d-d350e009366e", "Patient@Canc-app.com", true, null, false, null, "Mohamed Soliman", "PATIENT@CANC-APP.COM", "MOHAMED_SOLIMAN", "AQAAAAIAAYagAAAAEHE0DBT0NjWYFhPq9YZ+vvqMxtnnu6TJhDPJkR+b4kmd2XsQVzGNsVyXKnec9tlaDw==", null, false, "b9d50bbf5e87434fa03edc01413ca904", false, "Mohamed_Soliman", "Patient" },
                    { "6dc6528a-b280-4770-9eae-82671ee81ef7", 0, "Egypt, fayoum", "99d2bbc6-bc54-4248-a172-a77de3ae4430", "admin@Canc-app.com", true, null, false, null, "Mohamed Yasser", "ADMIN@CANC-APP.COM", "MOHAMED_YASSER", "AQAAAAIAAYagAAAAENb1rFSTXNwqK3luss66RiCZB1PZxAEXzpnh+mFV/clgdokmnyEzi8gmZ2fLp7r7Iw==", null, false, "55BF92C9EF0249CDA210D85D1A851BC9", false, "Mohamed_Yasser", "Admin" },
                    { "8dbabc2f-4dd1-4ef6-acb4-7feb3fe5ed13", 0, "Egypt, fayoum", "c97edf97-9d17-41f0-ae2c-04ff10d11c19", "Doctor@Canc-app.com", true, null, false, null, "Zyad Mahmoud", "DOCTOR@CANC-APP.COM", "ZYAD_MAHMOUD", "AQAAAAIAAYagAAAAEGD8LFyILYpwMSqmzV4KpUy5K8KvLUym/NX3IPFNAWoyzqdWTlxZXVWGdVoTuk+zlg==", null, false, "85a5af679fe14a24a17a7b9af212c4dc", false, "Zyad_Mahmoud", "Doctor" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: "18f2bb15-c5c3-4161-b3d5-ad8eab609da5");

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: "6dc6528a-b280-4770-9eae-82671ee81ef7");

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: "8dbabc2f-4dd1-4ef6-acb4-7feb3fe5ed13");

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
    }
}
