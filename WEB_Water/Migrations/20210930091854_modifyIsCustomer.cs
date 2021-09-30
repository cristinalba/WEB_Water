using Microsoft.EntityFrameworkCore.Migrations;

namespace WEB_Water.Migrations
{
    public partial class modifyIsCustomer : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ValueOfConsume",
                table: "Readings",
                newName: "ValueOfConsumption");

            migrationBuilder.AddColumn<bool>(
                name: "IsCustomer",
                table: "AspNetUsers",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsCustomer",
                table: "AspNetUsers");

            migrationBuilder.RenameColumn(
                name: "ValueOfConsumption",
                table: "Readings",
                newName: "ValueOfConsume");
        }
    }
}
