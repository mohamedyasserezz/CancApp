using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace CanaApp.Persistance.Data.Migrations
{
    /// <inheritdoc />
    public partial class SeedingData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "IsDefault", "IsDeleted", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "27e85d16-78f3-4e91-9985-3e80c7df31b9", "7ca43b8a-67ef-49d5-98a1-f03e7c9e0d42", false, false, "Psychiatrist", "PSYCHIATRIST" },
                    { "6f3a2b17-3c92-4d9d-8c24-77ba7cd0e851", "b71c4b95-2f28-4edd-9160-2b0e7d14e68f", false, false, "Pharmacist", "PHARMACIST" },
                    { "92b75286-d8f8-4061-9995-e6e23ccdee94", "f51e5a91-bced-49c2-8b86-c2e170c0846c", false, false, "Admin", "ADMIN" },
                    { "9eaa03df-8e4f-4161-85de-0f6e5e30bfd4", "5ee6bc12-5cb0-4304-91e7-6a00744e042a", false, false, "Doctor", "DOCTOR" },
                    { "d1fb4247-ccc5-4cb9-8c97-8c8c692e0e7a", "f5b8df6c-3f98-4b53-9a70-5f7e2c7f4759", false, false, "Volunteer", "VOLUNTEER" },
                    { "f739fe98-d8d7-4868-9b0c-51a9728863de", "65281949-fea3-4e86-a4b3-d4ca59e465a0", true, false, "Patient", "PATIENT" }
                });

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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "27e85d16-78f3-4e91-9985-3e80c7df31b9");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "6f3a2b17-3c92-4d9d-8c24-77ba7cd0e851");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "92b75286-d8f8-4061-9995-e6e23ccdee94");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "9eaa03df-8e4f-4161-85de-0f6e5e30bfd4");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "d1fb4247-ccc5-4cb9-8c97-8c8c692e0e7a");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "f739fe98-d8d7-4868-9b0c-51a9728863de");

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
        }
    }
}
