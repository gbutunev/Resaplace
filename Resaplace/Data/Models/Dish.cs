using System.ComponentModel.DataAnnotations;

namespace Resaplace.Data.Models
{
    public class Dish
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public double Price { get; set; }
        public Image Image { get; set; }
        public Restaurant Restaurant { get; set; }
    }
}