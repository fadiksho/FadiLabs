using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Modules.User.Features.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class RoleTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Auth0UserId",
                schema: "User",
                table: "LabUsers",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddColumn<Guid>(
                name: "LabRoleId",
                schema: "User",
                table: "LabUsers",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "LabRoles",
                schema: "User",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LabRoles", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_LabUsers_Auth0UserId",
                schema: "User",
                table: "LabUsers",
                column: "Auth0UserId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_LabUsers_LabRoleId",
                schema: "User",
                table: "LabUsers",
                column: "LabRoleId");

            migrationBuilder.AddForeignKey(
                name: "FK_LabUsers_LabRoles_LabRoleId",
                schema: "User",
                table: "LabUsers",
                column: "LabRoleId",
                principalSchema: "User",
                principalTable: "LabRoles",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_LabUsers_LabRoles_LabRoleId",
                schema: "User",
                table: "LabUsers");

            migrationBuilder.DropTable(
                name: "LabRoles",
                schema: "User");

            migrationBuilder.DropIndex(
                name: "IX_LabUsers_Auth0UserId",
                schema: "User",
                table: "LabUsers");

            migrationBuilder.DropIndex(
                name: "IX_LabUsers_LabRoleId",
                schema: "User",
                table: "LabUsers");

            migrationBuilder.DropColumn(
                name: "LabRoleId",
                schema: "User",
                table: "LabUsers");

            migrationBuilder.AlterColumn<string>(
                name: "Auth0UserId",
                schema: "User",
                table: "LabUsers",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");
        }
    }
}
