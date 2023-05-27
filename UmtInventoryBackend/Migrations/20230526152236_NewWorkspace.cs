using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace UmtInventoryBackend.Migrations
{
    /// <inheritdoc />
    public partial class NewWorkspace : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Users_Workspaces_WorkspaceID",
                table: "Users");

            migrationBuilder.DropIndex(
                name: "IX_Users_WorkspaceID",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "WorkspaceID",
                table: "Users");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "WorkspaceID",
                table: "Users",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Users_WorkspaceID",
                table: "Users",
                column: "WorkspaceID");

            migrationBuilder.AddForeignKey(
                name: "FK_Users_Workspaces_WorkspaceID",
                table: "Users",
                column: "WorkspaceID",
                principalTable: "Workspaces",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
