using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace Resaplace.Data.Models
{
    public class RestaurantStaff
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public IdentityUser Person { get; set; }
        [Required]
        public Restaurant Restaurant { get; set; }
    }
}
