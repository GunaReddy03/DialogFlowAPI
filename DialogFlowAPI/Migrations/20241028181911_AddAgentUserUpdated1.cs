using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DialogFlowAPI.Migrations
{
    /// <inheritdoc />
    public partial class AddAgentUserUpdated1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AgentUser_AspNetUsers_UserId1",
                table: "AgentUser");

            migrationBuilder.DropIndex(
                name: "IX_AgentUser_UserId1",
                table: "AgentUser");

            migrationBuilder.DropColumn(
                name: "UserId1",
                table: "AgentUser");

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "AgentUser",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<string>(
                name: "AgentID",
                table: "AgentUser",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.CreateIndex(
                name: "IX_AgentUser_UserId",
                table: "AgentUser",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_AgentUser_AspNetUsers_UserId",
                table: "AgentUser",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AgentUser_AspNetUsers_UserId",
                table: "AgentUser");

            migrationBuilder.DropIndex(
                name: "IX_AgentUser_UserId",
                table: "AgentUser");

            migrationBuilder.AlterColumn<int>(
                name: "UserId",
                table: "AgentUser",
                type: "int",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AlterColumn<int>(
                name: "AgentID",
                table: "AgentUser",
                type: "int",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddColumn<string>(
                name: "UserId1",
                table: "AgentUser",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_AgentUser_UserId1",
                table: "AgentUser",
                column: "UserId1");

            migrationBuilder.AddForeignKey(
                name: "FK_AgentUser_AspNetUsers_UserId1",
                table: "AgentUser",
                column: "UserId1",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }
    }
}
