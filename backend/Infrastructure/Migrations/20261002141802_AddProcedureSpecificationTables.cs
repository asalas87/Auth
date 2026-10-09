using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddProcedureSpecificationTables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ProcedureSpecifications",
                schema: "DOC",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "NEWSEQUENTIALID()"),
                    ProcedureNumber = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    StandardCode = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProcedureSpecifications", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProcedureSpecifications_DocumentFiles_Id",
                        column: x => x.Id,
                        principalSchema: "DOC",
                        principalTable: "DocumentFiles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ProcedureSpecificationRecords",
                schema: "DOC",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "NEWSEQUENTIALID()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProcedureSpecificationRecords", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProcedureSpecificationRecords_ProcedureSpecifications_Id",
                        column: x => x.Id,
                        principalSchema: "DOC",
                        principalTable: "ProcedureSpecifications",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ProcedureSpecificationRecords",
                schema: "DOC");

            migrationBuilder.DropTable(
                name: "ProcedureSpecifications",
                schema: "DOC");
        }
    }
}
