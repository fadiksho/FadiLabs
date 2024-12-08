using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Modules.User.Features.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "User");

            migrationBuilder.CreateTable(
                name: "LabRoles",
                schema: "User",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "NEWSEQUENTIALID()"),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AutoAssign = table.Column<bool>(type: "bit", nullable: false),
                    LabsPermissions = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LabRoles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "LabUsers",
                schema: "User",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Auth0UserId = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    DisplayName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Email = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    EmailVerified = table.Column<bool>(type: "bit", nullable: false),
                    ProfilePictureUrl = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LabUsers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "LabRoleLabUser",
                schema: "User",
                columns: table => new
                {
                    LabRolesId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    LabUsersId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LabRoleLabUser", x => new { x.LabRolesId, x.LabUsersId });
                    table.ForeignKey(
                        name: "FK_LabRoleLabUser_LabRoles_LabRolesId",
                        column: x => x.LabRolesId,
                        principalSchema: "User",
                        principalTable: "LabRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_LabRoleLabUser_LabUsers_LabUsersId",
                        column: x => x.LabUsersId,
                        principalSchema: "User",
                        principalTable: "LabUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                schema: "User",
                table: "LabRoles",
                columns: new[] { "Id", "AutoAssign", "Description", "LabsPermissions", "Name" },
                values: new object[] { new Guid("889027cf-742e-4ec6-a558-f526571819a7"), false, "default admin role.", -1, "admin" });

            migrationBuilder.CreateIndex(
                name: "IX_LabRoleLabUser_LabUsersId",
                schema: "User",
                table: "LabRoleLabUser",
                column: "LabUsersId");

            migrationBuilder.CreateIndex(
                name: "IX_LabRoles_Name",
                schema: "User",
                table: "LabRoles",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_LabUsers_Auth0UserId",
                schema: "User",
                table: "LabUsers",
                column: "Auth0UserId",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "LabRoleLabUser",
                schema: "User");

            migrationBuilder.DropTable(
                name: "LabRoles",
                schema: "User");

            migrationBuilder.DropTable(
                name: "LabUsers",
                schema: "User");
        }
    }
}
