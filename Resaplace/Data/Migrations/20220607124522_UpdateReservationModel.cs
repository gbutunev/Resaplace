using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Resaplace.Data.Migrations
{
    public partial class UpdateReservationModel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ReservationStatus",
                table: "Reservations",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ReservationStatus",
                table: "Reservations");
        }
    }
}
