using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace WEB_Water.Migrations
{
    public partial class modifyEntityReading : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "RegistrationDateNewReading",
                table: "Readings",
                type: "datetime2",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RegistrationDateNewReading",
                table: "Readings");
        }
    }
}
