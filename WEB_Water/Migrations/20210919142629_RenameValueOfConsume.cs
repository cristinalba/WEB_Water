using Microsoft.EntityFrameworkCore.Migrations;

namespace WEB_Water.Migrations
{
    public partial class RenameValueOfConsume : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "MonthlyConsume",
                table: "Readings",
                newName: "ValueOfConsume");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ValueOfConsume",
                table: "Readings",
                newName: "MonthlyConsume");
        }
    }
}
