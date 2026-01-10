using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class DocumentColumnsModifications : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Code",
                schema: "DOC",
                table: "Certificates");

            migrationBuilder.DropColumn(
                name: "EmployerName",
                schema: "DOC",
                table: "Certificates");

            migrationBuilder.RenameColumn(
                name: "ValidFrom",
                schema: "DOC",
                table: "Certificates",
                newName: "Validity");

            migrationBuilder.AlterColumn<string>(
                name: "CertificateNumber",
                schema: "DOC",
                table: "Certificates",
                type: "nvarchar(30)",
                maxLength: 30,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(200)",
                oldMaxLength: 200);

            migrationBuilder.AddColumn<string>(
                name: "EmployerFullName",
                schema: "DOC",
                table: "Certificates",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "StandardCode",
                schema: "DOC",
                table: "Certificates",
                type: "nvarchar(30)",
                maxLength: 30,
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EmployerFullName",
                schema: "DOC",
                table: "Certificates");

            migrationBuilder.DropColumn(
                name: "StandardCode",
                schema: "DOC",
                table: "Certificates");

            migrationBuilder.RenameColumn(
                name: "Validity",
                schema: "DOC",
                table: "Certificates",
                newName: "ValidFrom");

            migrationBuilder.AlterColumn<string>(
                name: "CertificateNumber",
                schema: "DOC",
                table: "Certificates",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(30)",
                oldMaxLength: 30);

            migrationBuilder.AddColumn<string>(
                name: "Code",
                schema: "DOC",
                table: "Certificates",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "EmployerName",
                schema: "DOC",
                table: "Certificates",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: false,
                defaultValue: "");
        }
    }
}
