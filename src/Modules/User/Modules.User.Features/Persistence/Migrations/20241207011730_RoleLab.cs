using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Modules.User.Features.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class RoleLab : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_LabRoles_Name",
                schema: "User",
                table: "LabRoles",
                column: "Name",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_LabRoles_Name",
                schema: "User",
                table: "LabRoles");
        }
    }
}
