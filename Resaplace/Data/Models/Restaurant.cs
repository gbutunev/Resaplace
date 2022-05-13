using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace Resaplace.Data.Models
{
    public class Restaurant
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Description { get; set; }
        [Required]
        public string Country { get; set; }
        [Required]
        public string City { get; set; }
        [Required]
        public string StreetAddress { get; set; }
        [Required]
        public int TotalSeats { get; set; }
        [Required]
        public int TotalTables { get; set; }
        [Required]
        public string PhoneNumber { get; set; }
        [Required]
        public IdentityUser Owner { get; set; }
        public List<Image> Images { get; set; }
        public List<Dish> Dishes { get; set; }
    }
}
