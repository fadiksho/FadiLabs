using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Modules.User.Features.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class UpdateLabRole : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Permissions",
                schema: "User",
                table: "LabRoles",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Permissions",
                schema: "User",
                table: "LabRoles");
        }
    }
}
