using System.ComponentModel.DataAnnotations;

namespace Resaplace.Data.Models
{
    public class Image
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string ImagePath { get; set; }
        [Required]
        public string AltText { get; set; }
    }
}
