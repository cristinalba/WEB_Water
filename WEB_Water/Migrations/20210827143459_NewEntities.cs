using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace WEB_Water.Migrations
{
    public partial class NewEntities : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Email",
                table: "Customers",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldMaxLength: 100,
                oldNullable: true);

            migrationBuilder.CreateTable(
                name: "Addresses",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CustomerDetailsId = table.Column<int>(type: "int", nullable: true),
                    AddressName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    PostalCode = table.Column<string>(type: "nvarchar(9)", maxLength: 9, nullable: true),
                    City = table.Column<string>(type: "nvarchar(70)", maxLength: 70, nullable: true),
                    BeginOfContract = table.Column<DateTime>(type: "datetime2", nullable: false),
                    NameOfBank = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Addresses", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Addresses_Customers_CustomerDetailsId",
                        column: x => x.CustomerDetailsId,
                        principalTable: "Customers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Readings",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AddressDetailsId = table.Column<int>(type: "int", nullable: true),
                    MonthlyReadingDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    MonthlyConsume = table.Column<double>(type: "float", nullable: false),
                    ValueToPay = table.Column<double>(type: "float", nullable: false),
                    WayOfPayment = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    StatusOfPayment = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Readings", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Readings_Addresses_AddressDetailsId",
                        column: x => x.AddressDetailsId,
                        principalTable: "Addresses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Addresses_CustomerDetailsId",
                table: "Addresses",
                column: "CustomerDetailsId");

            migrationBuilder.CreateIndex(
                name: "IX_Readings_AddressDetailsId",
                table: "Readings",
                column: "AddressDetailsId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Readings");

            migrationBuilder.DropTable(
                name: "Addresses");

            migrationBuilder.AlterColumn<string>(
                name: "Email",
                table: "Customers",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldMaxLength: 100);
        }
    }
}
