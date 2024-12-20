using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Modules.User.Features.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
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
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AutoAssign = table.Column<bool>(type: "bit", nullable: false),
                    LabsPermissions = table.Column<int>(type: "int", nullable: false),
                    Created = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    LastModified = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false)
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
                    ProfilePictureUrl = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Created = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    LastModified = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false)
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
                columns: new[] { "Id", "AutoAssign", "Created", "Description", "LabsPermissions", "LastModified", "Name" },
                values: new object[,]
                {
                    { new Guid("044e1975-75a6-4d16-b0b8-19c48fbd3377"), false, new DateTimeOffset(new DateTime(2024, 12, 20, 17, 51, 20, 980, DateTimeKind.Unspecified).AddTicks(6741), new TimeSpan(0, 0, 0, 0, 0)), "default admin role.", -1, new DateTimeOffset(new DateTime(2024, 12, 20, 17, 51, 20, 980, DateTimeKind.Unspecified).AddTicks(6931), new TimeSpan(0, 0, 0, 0, 0)), "admin" },
                    { new Guid("2cc3f5d7-4a52-484e-ae64-8613ef92e946"), true, new DateTimeOffset(new DateTime(2024, 12, 20, 17, 51, 20, 980, DateTimeKind.Unspecified).AddTicks(7091), new TimeSpan(0, 0, 0, 0, 0)), "default lab user role.", 0, new DateTimeOffset(new DateTime(2024, 12, 20, 17, 51, 20, 980, DateTimeKind.Unspecified).AddTicks(7092), new TimeSpan(0, 0, 0, 0, 0)), "lab user" }
                });

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
