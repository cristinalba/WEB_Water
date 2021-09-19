using Microsoft.EntityFrameworkCore.Migrations;

namespace WEB_Water.Migrations
{
    public partial class modifyUser : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Readers_AspNetUsers_UserId",
                table: "Readers");

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "Readers",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Readers_AspNetUsers_UserId",
                table: "Readers",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Readers_AspNetUsers_UserId",
                table: "Readers");

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "Readers",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AddForeignKey(
                name: "FK_Readers_AspNetUsers_UserId",
                table: "Readers",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
