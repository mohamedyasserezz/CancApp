using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CanaApp.Persistance.Data.Migrations
{
    /// <inheritdoc />
    public partial class RemoveNavigtionalPropertyAboutCommunityFromUsers : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Comments_Doctors_UserId",
                table: "Comments");

            migrationBuilder.DropForeignKey(
                name: "FK_Comments_Patients_UserId",
                table: "Comments");

            migrationBuilder.DropForeignKey(
                name: "FK_Comments_Psychiatrists_UserId",
                table: "Comments");

            migrationBuilder.DropForeignKey(
                name: "FK_Comments_Volunteers_UserId",
                table: "Comments");

            migrationBuilder.DropForeignKey(
                name: "FK_Posts_Doctors_UserId",
                table: "Posts");

            migrationBuilder.DropForeignKey(
                name: "FK_Posts_Patients_UserId",
                table: "Posts");

            migrationBuilder.DropForeignKey(
                name: "FK_Posts_Psychiatrists_UserId",
                table: "Posts");

            migrationBuilder.DropForeignKey(
                name: "FK_Posts_Volunteers_UserId",
                table: "Posts");

            migrationBuilder.DropForeignKey(
                name: "FK_Reactions_Doctors_UserId",
                table: "Reactions");

            migrationBuilder.DropForeignKey(
                name: "FK_Reactions_Patients_UserId",
                table: "Reactions");

            migrationBuilder.DropForeignKey(
                name: "FK_Reactions_Psychiatrists_UserId",
                table: "Reactions");

            migrationBuilder.DropForeignKey(
                name: "FK_Reactions_Volunteers_UserId",
                table: "Reactions");

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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
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

            migrationBuilder.AddForeignKey(
                name: "FK_Comments_Doctors_UserId",
                table: "Comments",
                column: "UserId",
                principalTable: "Doctors",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Comments_Patients_UserId",
                table: "Comments",
                column: "UserId",
                principalTable: "Patients",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Comments_Psychiatrists_UserId",
                table: "Comments",
                column: "UserId",
                principalTable: "Psychiatrists",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Comments_Volunteers_UserId",
                table: "Comments",
                column: "UserId",
                principalTable: "Volunteers",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Posts_Doctors_UserId",
                table: "Posts",
                column: "UserId",
                principalTable: "Doctors",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Posts_Patients_UserId",
                table: "Posts",
                column: "UserId",
                principalTable: "Patients",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Posts_Psychiatrists_UserId",
                table: "Posts",
                column: "UserId",
                principalTable: "Psychiatrists",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Posts_Volunteers_UserId",
                table: "Posts",
                column: "UserId",
                principalTable: "Volunteers",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Reactions_Doctors_UserId",
                table: "Reactions",
                column: "UserId",
                principalTable: "Doctors",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Reactions_Patients_UserId",
                table: "Reactions",
                column: "UserId",
                principalTable: "Patients",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Reactions_Psychiatrists_UserId",
                table: "Reactions",
                column: "UserId",
                principalTable: "Psychiatrists",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Reactions_Volunteers_UserId",
                table: "Reactions",
                column: "UserId",
                principalTable: "Volunteers",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
