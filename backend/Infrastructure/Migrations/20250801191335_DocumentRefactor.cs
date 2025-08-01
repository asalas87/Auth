using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class DocumentRefactor : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DocumentFiles_Users_AssignedToId",
                schema: "DOC",
                table: "DocumentFiles");

            migrationBuilder.AddColumn<int>(
                name: "DocumentType",
                schema: "DOC",
                table: "DocumentFiles",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "Certificates",
                schema: "DOC",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ValidFrom = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ValidUntil = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Certificates", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Certificates_DocumentFiles_Id",
                        column: x => x.Id,
                        principalSchema: "DOC",
                        principalTable: "DocumentFiles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "GeneralDocuments",
                schema: "DOC",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GeneralDocuments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_GeneralDocuments_DocumentFiles_Id",
                        column: x => x.Id,
                        principalSchema: "DOC",
                        principalTable: "DocumentFiles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.AddForeignKey(
                name: "FK_DocumentFiles_Companies_AssignedToId",
                schema: "DOC",
                table: "DocumentFiles",
                column: "AssignedToId",
                principalSchema: "PAR",
                principalTable: "Companies",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DocumentFiles_Companies_AssignedToId",
                schema: "DOC",
                table: "DocumentFiles");

            migrationBuilder.DropTable(
                name: "Certificates",
                schema: "DOC");

            migrationBuilder.DropTable(
                name: "GeneralDocuments",
                schema: "DOC");

            migrationBuilder.DropColumn(
                name: "DocumentType",
                schema: "DOC",
                table: "DocumentFiles");

            migrationBuilder.AddForeignKey(
                name: "FK_DocumentFiles_Users_AssignedToId",
                schema: "DOC",
                table: "DocumentFiles",
                column: "AssignedToId",
                principalSchema: "SEC",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
