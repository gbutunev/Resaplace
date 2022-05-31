using Microsoft.AspNetCore.Identity;

namespace Resaplace.Data.Models
{
    public class ReservationSubmition
    {
        public IdentityUser User { get; set; }
        public DateTime DateTime { get; set; }
        public List<Dish> Dishes { get; set; }
        public int GuestNumber { get; set; }
    }
}
