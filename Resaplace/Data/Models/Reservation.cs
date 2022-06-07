using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace Resaplace.Data.Models
{
    public class Reservation
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public IdentityUser User { get; set; }
        [Required]
        public DateTime DateTime { get; set; }
        [Required]
        [Range(1,12)]
        public int PeopleNumber { get; set; }
        [Required]
        public Restaurant Restaurant { get; set; }
        public string Message { get; set; }
        public virtual ICollection<ReservationDish> ReservationDishes { get; set; }
        public BasicStatus ReservationStatus { get; set; }
    }
}
