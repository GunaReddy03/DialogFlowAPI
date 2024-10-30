using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DialogFlowAPI.Migrations
{
    /// <inheritdoc />
    public partial class AddAgentUserUpdated : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
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
                defaultValue: 0,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
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
                nullable: true,
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
                principalColumn: "Id");
        }
    }
}
