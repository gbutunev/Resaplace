using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Resaplace.Data.Migrations
{
    public partial class ExpandRestaurantApplication : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "PhoneNumber",
                table: "RestaurantApplications",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "TotalSeats",
                table: "RestaurantApplications",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "TotalTables",
                table: "RestaurantApplications",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PhoneNumber",
                table: "RestaurantApplications");

            migrationBuilder.DropColumn(
                name: "TotalSeats",
                table: "RestaurantApplications");

            migrationBuilder.DropColumn(
                name: "TotalTables",
                table: "RestaurantApplications");
        }
    }
}
