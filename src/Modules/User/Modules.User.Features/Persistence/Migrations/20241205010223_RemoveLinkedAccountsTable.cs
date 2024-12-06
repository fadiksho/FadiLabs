using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Modules.User.Features.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class RemoveLinkedAccountsTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "LinkedAccount",
                schema: "User");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "LinkedAccount",
                schema: "User",
                columns: table => new
                {
                    Provider = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: false),
                    ProviderKey = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: false),
                    Connection = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsSocial = table.Column<bool>(type: "bit", nullable: false),
                    LabUserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LinkedAccount", x => new { x.Provider, x.ProviderKey });
                    table.ForeignKey(
                        name: "FK_LinkedAccount_LabUsers_LabUserId",
                        column: x => x.LabUserId,
                        principalSchema: "User",
                        principalTable: "LabUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_LinkedAccount_LabUserId",
                schema: "User",
                table: "LinkedAccount",
                column: "LabUserId");
        }
    }
}
