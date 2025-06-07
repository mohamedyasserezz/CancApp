using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CanaApp.Persistance.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddIsDisabledAttributeForUser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsDisaired",
                table: "Users",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: "18f2bb15-c5c3-4161-b3d5-ad8eab609da5",
                columns: new[] { "IsDisaired", "PasswordHash" },
                values: new object[] { false, "AQAAAAIAAYagAAAAEFo55a6l7CvKcmEEgARTCHDmpvaGbw59oc0o2BXBqivOR7lM9D8HlwMRfJOqYxmKsw==" });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: "6dc6528a-b280-4770-9eae-82671ee81ef7",
                columns: new[] { "IsDisaired", "PasswordHash" },
                values: new object[] { false, "AQAAAAIAAYagAAAAEF0nMqtlrJ93lO2+Zn4XMqFOq06xAppJHI2NkZobxgrVf5RRud3TMTknEA2UAqP+LA==" });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: "8dbabc2f-4dd1-4ef6-acb4-7feb3fe5ed13",
                columns: new[] { "IsDisaired", "PasswordHash" },
                values: new object[] { false, "AQAAAAIAAYagAAAAEKgMO1cd3c3wreDaoFprufUvqMVxEZcPdLcD5Y1T4F+SVyvOpCpgfvLIZ6sElE+E7A==" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsDisaired",
                table: "Users");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: "18f2bb15-c5c3-4161-b3d5-ad8eab609da5",
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAEA6nn3G8KwW6i2s4gYXZOtIEPsbrDSDFHrcG+IcEYlqiqjcE2uTPNxcjgr7HkbDqqQ==");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: "6dc6528a-b280-4770-9eae-82671ee81ef7",
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAEPheInTTV5ubXlDZpHfyxulnV6bjifCJgSLdQQjmn0PJPBksCX472/hAfVdBzTyxbg==");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: "8dbabc2f-4dd1-4ef6-acb4-7feb3fe5ed13",
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAEFdEnT3JqKjErcZw9nA4EFNCDuz7fuKOYCw1i3B2EbeKS1uybm9WTz+NQoVqCPcB+w==");
        }
    }
}
