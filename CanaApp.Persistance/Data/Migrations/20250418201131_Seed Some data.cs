using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CanaApp.Persistance.Data.Migrations
{
    /// <inheritdoc />
    public partial class SeedSomedata : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Doctors",
                columns: new[] { "UserId", "ImageId", "IsCompletedProfileFailed", "IsConfirmedByAdmin", "IsDisabled", "MedicalSyndicatePhoto", "NumberOfWarrings" },
                values: new object[] { "8dbabc2f-4dd1-4ef6-acb4-7feb3fe5ed13", null, false, true, false, null, 0 });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: "18f2bb15-c5c3-4161-b3d5-ad8eab609da5",
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAECXvVkryTCAqbZRa/8qz5dEmytajg27bsCtXGCHhknJ55L11xCEUM+7LaQ46gdxdug==");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: "6dc6528a-b280-4770-9eae-82671ee81ef7",
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAEBbyV7K7R8KoduH8LBpehoxZwVQHfvc4p4Vq2t0rspBiUU6SErmvzv6qgviK7ZhbEQ==");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: "8dbabc2f-4dd1-4ef6-acb4-7feb3fe5ed13",
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAEIt4qw60TS5Vwht5dwk9SLQud/gxaZB+i2v2Cfd7CtshrlHeb9NgA6mapVUgRmyfAg==");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Doctors",
                keyColumn: "UserId",
                keyValue: "8dbabc2f-4dd1-4ef6-acb4-7feb3fe5ed13");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: "18f2bb15-c5c3-4161-b3d5-ad8eab609da5",
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAEM/Op1GRcBuH+hkHIsCk7tvyOHoa5IDp18YebHajnq0rb1ftXAhpeVxpsQcGS9qM0g==");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: "6dc6528a-b280-4770-9eae-82671ee81ef7",
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAEG9ruFymg71rju2SrvzlwuJFqaI+U7TT4CXshBB9aTNpZnnkrrz96MptJsVr7WUMLg==");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: "8dbabc2f-4dd1-4ef6-acb4-7feb3fe5ed13",
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAEHipeCMYKhb9QV6KAn7sASctY09IK+QUYJAvmU4HVCPwJLUUUd2C4juuK3YigFGQaw==");
        }
    }
}
