using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class FixFkDeleteBehaviors : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DocumentFiles_Companies_AssignedToId",
                schema: "DOC",
                table: "DocumentFiles");

            migrationBuilder.DropForeignKey(
                name: "FK_Users_Companies_CompanyId",
                schema: "SEC",
                table: "Users");

            migrationBuilder.AlterColumn<Guid>(
                name: "CompanyId",
                schema: "SEC",
                table: "Users",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.CreateIndex(
                name: "IX_UserActivationToken_UserId",
                schema: "SEC",
                table: "UserActivationToken",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_DocumentFiles_Companies_AssignedToId",
                schema: "DOC",
                table: "DocumentFiles",
                column: "AssignedToId",
                principalSchema: "PAR",
                principalTable: "Companies",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_UserActivationToken_Users_UserId",
                schema: "SEC",
                table: "UserActivationToken",
                column: "UserId",
                principalSchema: "SEC",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Users_Companies_CompanyId",
                schema: "SEC",
                table: "Users",
                column: "CompanyId",
                principalSchema: "PAR",
                principalTable: "Companies",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DocumentFiles_Companies_AssignedToId",
                schema: "DOC",
                table: "DocumentFiles");

            migrationBuilder.DropForeignKey(
                name: "FK_UserActivationToken_Users_UserId",
                schema: "SEC",
                table: "UserActivationToken");

            migrationBuilder.DropForeignKey(
                name: "FK_Users_Companies_CompanyId",
                schema: "SEC",
                table: "Users");

            migrationBuilder.DropIndex(
                name: "IX_UserActivationToken_UserId",
                schema: "SEC",
                table: "UserActivationToken");

            migrationBuilder.AlterColumn<Guid>(
                name: "CompanyId",
                schema: "SEC",
                table: "Users",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_DocumentFiles_Companies_AssignedToId",
                schema: "DOC",
                table: "DocumentFiles",
                column: "AssignedToId",
                principalSchema: "PAR",
                principalTable: "Companies",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Users_Companies_CompanyId",
                schema: "SEC",
                table: "Users",
                column: "CompanyId",
                principalSchema: "PAR",
                principalTable: "Companies",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
