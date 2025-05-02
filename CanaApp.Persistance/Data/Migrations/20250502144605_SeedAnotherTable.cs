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

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: "18f2bb15-c5c3-4161-b3d5-ad8eab609da5",
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAEFZcOdOdmUm+e/o1dpXBY8iePGH4dqt0Ik5+1WloFlpW3sN6wwa7nAtsXbDqn14wRg==");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: "6dc6528a-b280-4770-9eae-82671ee81ef7",
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAEIxMD/rUFlEww+VqifGOTf0/HzKMIomFCIARSUfkDAQbru9SShydDZsEbzdAqvCeoA==");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: "8dbabc2f-4dd1-4ef6-acb4-7feb3fe5ed13",
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAEH9wh9p/Sre/HP8rRO0/sl6iv2FbARnEC4iuKQZ0e1PYx07gk+RB/dgw2eB0QCu9Nw==");
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

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: "18f2bb15-c5c3-4161-b3d5-ad8eab609da5",
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAEAehWUWWZy4LgE/J3gF6mfOaK+7AyNfJnKvcL1dsTgyzp7b1zJF5ZgSBUO+R4De6VQ==");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: "6dc6528a-b280-4770-9eae-82671ee81ef7",
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAEOaCpUZfPkRM1tkPVW+0Kk4Hz/m65jMlb9NNJcru929pR2bzSTpaZbMrYFo3PNNDfw==");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: "8dbabc2f-4dd1-4ef6-acb4-7feb3fe5ed13",
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAEBI/PZSjI/fqyoXMoQUACLMVXiU33aJWyCeUkvC95zrPy/PBIbzZaanncUcVt1voiA==");
        }
    }
}
