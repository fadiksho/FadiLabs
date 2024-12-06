using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Modules.User.Features.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class UpdateLabRoleAndLabUser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_LabUsers_LabRoles_LabRoleId",
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
                name: "Email",
                schema: "User",
                table: "LabUsers",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "Auth0UserId",
                schema: "User",
                table: "LabUsers",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                schema: "User",
                table: "LabRoles",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddColumn<string>(
                name: "Description",
                schema: "User",
                table: "LabRoles",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateTable(
                name: "LabRoleLabUser",
                schema: "User",
                columns: table => new
                {
                    LabRolesId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UsersId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LabRoleLabUser", x => new { x.LabRolesId, x.UsersId });
                    table.ForeignKey(
                        name: "FK_LabRoleLabUser_LabRoles_LabRolesId",
                        column: x => x.LabRolesId,
                        principalSchema: "User",
                        principalTable: "LabRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_LabRoleLabUser_LabUsers_UsersId",
                        column: x => x.UsersId,
                        principalSchema: "User",
                        principalTable: "LabUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_LabRoleLabUser_UsersId",
                schema: "User",
                table: "LabRoleLabUser",
                column: "UsersId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "LabRoleLabUser",
                schema: "User");

            migrationBuilder.DropColumn(
                name: "Description",
                schema: "User",
                table: "LabRoles");

            migrationBuilder.AlterColumn<string>(
                name: "Email",
                schema: "User",
                table: "LabUsers",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldMaxLength: 50);

            migrationBuilder.AlterColumn<string>(
                name: "Auth0UserId",
                schema: "User",
                table: "LabUsers",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldMaxLength: 50);

            migrationBuilder.AddColumn<Guid>(
                name: "LabRoleId",
                schema: "User",
                table: "LabUsers",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                schema: "User",
                table: "LabRoles",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldMaxLength: 50);

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
    }
}
