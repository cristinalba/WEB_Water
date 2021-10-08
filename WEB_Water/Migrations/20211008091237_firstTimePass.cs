using Microsoft.EntityFrameworkCore.Migrations;

namespace WEB_Water.Migrations
{
    public partial class firstTimePass : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "FirstTimePass",
                table: "AspNetUsers",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FirstTimePass",
                table: "AspNetUsers");
        }
    }
}
