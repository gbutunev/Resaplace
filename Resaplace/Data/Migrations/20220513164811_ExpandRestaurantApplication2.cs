using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Resaplace.Data.Migrations
{
    public partial class ExpandRestaurantApplication2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "FeedbackMessage",
                table: "RestaurantApplications",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FeedbackMessage",
                table: "RestaurantApplications");
        }
    }
}
