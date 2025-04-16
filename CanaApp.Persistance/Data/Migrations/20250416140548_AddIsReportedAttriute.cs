using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CanaApp.Persistance.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddIsReportedAttriute : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Comments_Comments_ParentCommentId",
                table: "Comments");

            migrationBuilder.DropIndex(
                name: "IX_Comments_ParentCommentId",
                table: "Comments");

            migrationBuilder.DropColumn(
                name: "ParentCommentId",
                table: "Comments");

            migrationBuilder.AddColumn<int>(
                name: "NumberOfWarrings",
                table: "Users",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<int>(
                name: "CommentId",
                table: "Reactions",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<bool>(
                name: "IsReported",
                table: "Posts",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsReported",
                table: "Comments",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: "18f2bb15-c5c3-4161-b3d5-ad8eab609da5",
                columns: new[] { "NumberOfWarrings", "PasswordHash" },
                values: new object[] { 0, "AQAAAAIAAYagAAAAELEs58Ys51yczZr76WLy8AQ9p8tj08egVbTpjF+goI9TdjVRIVMWfRDPZQdne5n0bg==" });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: "6dc6528a-b280-4770-9eae-82671ee81ef7",
                columns: new[] { "NumberOfWarrings", "PasswordHash" },
                values: new object[] { 0, "AQAAAAIAAYagAAAAEGcq6RVWo9FqFL8i1K9SV0XnKuoPk/dcrxHbwkxudEJFDMpjdm2t+RYA75aTDc83pA==" });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: "8dbabc2f-4dd1-4ef6-acb4-7feb3fe5ed13",
                columns: new[] { "NumberOfWarrings", "PasswordHash" },
                values: new object[] { 0, "AQAAAAIAAYagAAAAEJ/DISt0SkByoutDWuRzFyAdOSl8lAYvcaZ3cbA2nmqHCoh6i5ptub/+Zjmz0PWF+w==" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "NumberOfWarrings",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "IsReported",
                table: "Posts");

            migrationBuilder.DropColumn(
                name: "IsReported",
                table: "Comments");

            migrationBuilder.AlterColumn<int>(
                name: "CommentId",
                table: "Reactions",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ParentCommentId",
                table: "Comments",
                type: "int",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: "18f2bb15-c5c3-4161-b3d5-ad8eab609da5",
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAEEoSdn+hW2ioLvcJA4SVCkO0zniro25LRnPAjcC8yHkFrMOzFYbq4oAbbfvlzLUCRg==");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: "6dc6528a-b280-4770-9eae-82671ee81ef7",
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAEJhCqL+Y3zJ9Gx3thpcw8H6ZvrvlBBjixKPCEIvGwFfLhuKEqclZwPabQhnR34qUcw==");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: "8dbabc2f-4dd1-4ef6-acb4-7feb3fe5ed13",
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAELp7aup1TNr+B/CEKAzOxmMjHiQwBTprQeg1JX1EJgZj7gmvs5vor7TCr5i0Ni/vBQ==");

            migrationBuilder.CreateIndex(
                name: "IX_Comments_ParentCommentId",
                table: "Comments",
                column: "ParentCommentId");

            migrationBuilder.AddForeignKey(
                name: "FK_Comments_Comments_ParentCommentId",
                table: "Comments",
                column: "ParentCommentId",
                principalTable: "Comments",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
