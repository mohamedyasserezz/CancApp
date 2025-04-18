using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CanaApp.Persistance.Data.Migrations
{
    /// <inheritdoc />
    public partial class ChangeinSomeAttributesforconfirmtheidentity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsDisabled",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "NumberOfWarrings",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "MedicalAssociationId",
                table: "Psychiatrists");

            migrationBuilder.DropColumn(
                name: "LicenseNumber",
                table: "Pharmacists");

            migrationBuilder.DropColumn(
                name: "MedicalAssociationId",
                table: "Doctors");

            migrationBuilder.AddColumn<bool>(
                name: "IsDisabled",
                table: "Volunteers",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "NumberOfWarrings",
                table: "Volunteers",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "ImageId",
                table: "Psychiatrists",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "MedicalSyndicatePhoto",
                table: "Psychiatrists",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "ImageId",
                table: "Pharmacists",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "ImagePharmacyLicense",
                table: "Pharmacists",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<bool>(
                name: "IsDeliveryEnabled",
                table: "Pharmacists",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsDisabled",
                table: "Pharmacists",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "NumberOfWarrings",
                table: "Pharmacists",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "NumberOfWorkingHours",
                table: "Pharmacists",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<bool>(
                name: "IsDisabled",
                table: "Patients",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "NumberOfWarrings",
                table: "Patients",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "ImageId",
                table: "Doctors",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<bool>(
                name: "IsDisabled",
                table: "Doctors",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "MedicalSyndicatePhoto",
                table: "Doctors",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "NumberOfWarrings",
                table: "Doctors",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: "18f2bb15-c5c3-4161-b3d5-ad8eab609da5",
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAEKQnFlWJtmBpbhDYlTy1MG/wUizMXEkug3YrXYkPc3ZS7kQUdJ7/djUnGPzemBzjpw==");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: "6dc6528a-b280-4770-9eae-82671ee81ef7",
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAEJSmKT7GfcCWMJXQ9hI+3cazf4PM/5wW/SGZ0mTZRdPRGU/0rKlVJuwddbW09YvLyg==");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: "8dbabc2f-4dd1-4ef6-acb4-7feb3fe5ed13",
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAEGFTU0fK0kZeVjV4upGvDRK3bwKPhvMi8dw13x5F/jjVPpHaBa73dsbfWbK8zPny1g==");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsDisabled",
                table: "Volunteers");

            migrationBuilder.DropColumn(
                name: "NumberOfWarrings",
                table: "Volunteers");

            migrationBuilder.DropColumn(
                name: "ImageId",
                table: "Psychiatrists");

            migrationBuilder.DropColumn(
                name: "MedicalSyndicatePhoto",
                table: "Psychiatrists");

            migrationBuilder.DropColumn(
                name: "ImageId",
                table: "Pharmacists");

            migrationBuilder.DropColumn(
                name: "ImagePharmacyLicense",
                table: "Pharmacists");

            migrationBuilder.DropColumn(
                name: "IsDeliveryEnabled",
                table: "Pharmacists");

            migrationBuilder.DropColumn(
                name: "IsDisabled",
                table: "Pharmacists");

            migrationBuilder.DropColumn(
                name: "NumberOfWarrings",
                table: "Pharmacists");

            migrationBuilder.DropColumn(
                name: "NumberOfWorkingHours",
                table: "Pharmacists");

            migrationBuilder.DropColumn(
                name: "IsDisabled",
                table: "Patients");

            migrationBuilder.DropColumn(
                name: "NumberOfWarrings",
                table: "Patients");

            migrationBuilder.DropColumn(
                name: "ImageId",
                table: "Doctors");

            migrationBuilder.DropColumn(
                name: "IsDisabled",
                table: "Doctors");

            migrationBuilder.DropColumn(
                name: "MedicalSyndicatePhoto",
                table: "Doctors");

            migrationBuilder.DropColumn(
                name: "NumberOfWarrings",
                table: "Doctors");

            migrationBuilder.AddColumn<bool>(
                name: "IsDisabled",
                table: "Users",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "NumberOfWarrings",
                table: "Users",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "MedicalAssociationId",
                table: "Psychiatrists",
                type: "nvarchar(150)",
                maxLength: 150,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "LicenseNumber",
                table: "Pharmacists",
                type: "nvarchar(150)",
                maxLength: 150,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "MedicalAssociationId",
                table: "Doctors",
                type: "nvarchar(150)",
                maxLength: 150,
                nullable: false,
                defaultValue: "");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: "18f2bb15-c5c3-4161-b3d5-ad8eab609da5",
                columns: new[] { "IsDisabled", "NumberOfWarrings", "PasswordHash" },
                values: new object[] { false, 0, "AQAAAAIAAYagAAAAEDPTfCZywzaPVHS3tkEdDuLSMOgc3C/wHYvcHeYhBJCHFQB9mnZH1BEvggfg2Y1t9w==" });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: "6dc6528a-b280-4770-9eae-82671ee81ef7",
                columns: new[] { "IsDisabled", "NumberOfWarrings", "PasswordHash" },
                values: new object[] { false, 0, "AQAAAAIAAYagAAAAEFCohS9SHP+eLP7F8WaSUBu5Ir8cOVvQpmgwZt1nycY8UG8S0KqiwUajKZyzw97HCg==" });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: "8dbabc2f-4dd1-4ef6-acb4-7feb3fe5ed13",
                columns: new[] { "IsDisabled", "NumberOfWarrings", "PasswordHash" },
                values: new object[] { false, 0, "AQAAAAIAAYagAAAAEJ7bWzHnZ8F25I07NLKEH8wdpK7hbV5OyF0952743+ii3TIBJWYf7WVZcMrJlBgl5Q==" });
        }
    }
}
