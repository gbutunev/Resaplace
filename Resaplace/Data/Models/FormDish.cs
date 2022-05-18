using Microsoft.AspNetCore.Components.Forms;
using System.ComponentModel.DataAnnotations;

namespace Resaplace.Data.Models
{
    public class FormDish
    {
        [Required(ErrorMessage = "Името е задължително!")]
        public string Name { get; set; }
        [Required(ErrorMessage = "Цената е задължителна!")]
        [Range(0, 100, ErrorMessage = "Цената трябва да е между 0 и 100!")]
        public double Price { get; set; }
        public IBrowserFile Image { get; set; }
    }
}
