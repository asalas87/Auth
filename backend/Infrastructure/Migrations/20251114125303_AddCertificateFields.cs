using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddCertificateFields : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ValidUntil",
                schema: "DOC",
                table: "Certificates");

            migrationBuilder.AddColumn<bool>(
                name: "IsRead",
                schema: "DOC",
                table: "DocumentFiles",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "CertificateNumber",
                schema: "DOC",
                table: "Certificates",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Code",
                schema: "DOC",
                table: "Certificates",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "EmployerName",
                schema: "DOC",
                table: "Certificates",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsRead",
                schema: "DOC",
                table: "DocumentFiles");

            migrationBuilder.DropColumn(
                name: "CertificateNumber",
                schema: "DOC",
                table: "Certificates");

            migrationBuilder.DropColumn(
                name: "Code",
                schema: "DOC",
                table: "Certificates");

            migrationBuilder.DropColumn(
                name: "EmployerName",
                schema: "DOC",
                table: "Certificates");

            migrationBuilder.AddColumn<DateTime>(
                name: "ValidUntil",
                schema: "DOC",
                table: "Certificates",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }
    }
}
