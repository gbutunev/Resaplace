using System.ComponentModel.DataAnnotations;

namespace Resaplace.Data.Models
{
    public class ReservationDish
    {
        [Key]
        public int Id { get; set; }
        public int ReservationId { get; set; }
        public Reservation Reservation { get; set; }
        public int DishId { get; set; }
        public Dish Dish { get; set; }
        public int Amount { get; set; }

    }
}
