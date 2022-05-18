using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Resaplace.Data.Migrations
{
    public partial class UpdateDishes : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<double>(
                name: "Price",
                table: "Dishes",
                type: "float",
                nullable: false,
                defaultValue: 0.0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Price",
                table: "Dishes");
        }
    }
}
