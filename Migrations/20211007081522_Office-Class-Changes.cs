using Microsoft.EntityFrameworkCore.Migrations;

namespace MiniProject_02_EF_AssetTracking.Migrations
{
    public partial class OfficeClassChanges : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Currency",
                table: "Offices",
                newName: "CurrencySymbol");

            migrationBuilder.RenameColumn(
                name: "ConvertedPrice",
                table: "Offices",
                newName: "ExchangeRate");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ExchangeRate",
                table: "Offices",
                newName: "ConvertedPrice");

            migrationBuilder.RenameColumn(
                name: "CurrencySymbol",
                table: "Offices",
                newName: "Currency");
        }
    }
}
