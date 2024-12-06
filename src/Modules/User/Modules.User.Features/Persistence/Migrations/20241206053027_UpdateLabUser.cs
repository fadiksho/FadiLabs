using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Modules.User.Features.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class UpdateLabUser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_LabRoleLabUser_LabUsers_UsersId",
                schema: "User",
                table: "LabRoleLabUser");

            migrationBuilder.RenameColumn(
                name: "UsersId",
                schema: "User",
                table: "LabRoleLabUser",
                newName: "LabUsersId");

            migrationBuilder.RenameIndex(
                name: "IX_LabRoleLabUser_UsersId",
                schema: "User",
                table: "LabRoleLabUser",
                newName: "IX_LabRoleLabUser_LabUsersId");

            migrationBuilder.AddColumn<string>(
                name: "DisplayName",
                schema: "User",
                table: "LabUsers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "EmailVerified",
                schema: "User",
                table: "LabUsers",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddForeignKey(
                name: "FK_LabRoleLabUser_LabUsers_LabUsersId",
                schema: "User",
                table: "LabRoleLabUser",
                column: "LabUsersId",
                principalSchema: "User",
                principalTable: "LabUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_LabRoleLabUser_LabUsers_LabUsersId",
                schema: "User",
                table: "LabRoleLabUser");

            migrationBuilder.DropColumn(
                name: "DisplayName",
                schema: "User",
                table: "LabUsers");

            migrationBuilder.DropColumn(
                name: "EmailVerified",
                schema: "User",
                table: "LabUsers");

            migrationBuilder.RenameColumn(
                name: "LabUsersId",
                schema: "User",
                table: "LabRoleLabUser",
                newName: "UsersId");

            migrationBuilder.RenameIndex(
                name: "IX_LabRoleLabUser_LabUsersId",
                schema: "User",
                table: "LabRoleLabUser",
                newName: "IX_LabRoleLabUser_UsersId");

            migrationBuilder.AddForeignKey(
                name: "FK_LabRoleLabUser_LabUsers_UsersId",
                schema: "User",
                table: "LabRoleLabUser",
                column: "UsersId",
                principalSchema: "User",
                principalTable: "LabUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
