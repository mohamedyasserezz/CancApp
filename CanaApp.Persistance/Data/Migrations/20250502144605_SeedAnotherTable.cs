using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace CanaApp.Persistance.Data.Migrations
{
    /// <inheritdoc />
    public partial class SeedAnotherTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Image",
                table: "Posts",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.InsertData(
                table: "AspNetUserRoles",
                columns: new[] { "RoleId", "UserId" },
                values: new object[,]
                {
                    { "f739fe98-d8d7-4868-9b0c-51a9728863de", "18f2bb15-c5c3-4161-b3d5-ad8eab609da5" },
                    { "92b75286-d8f8-4061-9995-e6e23ccdee94", "6dc6528a-b280-4770-9eae-82671ee81ef7" },
                    { "9eaa03df-8e4f-4161-85de-0f6e5e30bfd4", "8dbabc2f-4dd1-4ef6-acb4-7feb3fe5ed13" }
                });

        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { "f739fe98-d8d7-4868-9b0c-51a9728863de", "18f2bb15-c5c3-4161-b3d5-ad8eab609da5" });

            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { "92b75286-d8f8-4061-9995-e6e23ccdee94", "6dc6528a-b280-4770-9eae-82671ee81ef7" });


            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { "9eaa03df-8e4f-4161-85de-0f6e5e30bfd4", "8dbabc2f-4dd1-4ef6-acb4-7feb3fe5ed13" });

            migrationBuilder.DropColumn(
                name: "Image",
                table: "Posts");

        }
    }
}
