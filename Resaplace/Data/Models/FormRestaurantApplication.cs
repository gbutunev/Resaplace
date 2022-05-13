using Microsoft.AspNetCore.Components.Forms;
using System.ComponentModel.DataAnnotations;

namespace Resaplace.Data.Models
{
    public class FormRestaurantApplication
    {
        [Required(ErrorMessage = "Името е задължително!")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Описанието е задължително!")]
        public string Description { get; set; }

        [Required(ErrorMessage = "Държавата е задължителна!")]
        public string Country { get; set; }

        [Required(ErrorMessage = "Градът е задължителен!")]
        public string City { get; set; }

        [Required(ErrorMessage = "Адресът е задължителен!")]
        public string StreetAddress { get; set; }

        [Required(ErrorMessage = "Броят на местата е задължителен!")]
        [Range(4, 400, ErrorMessage = "Броят на местата трябва да е между 4 и 400!")]
        public int TotalSeats { get; set; }

        [Required(ErrorMessage = "Броят на масите е задъжителен!")]
        [Range(1, 100, ErrorMessage = "Броят на масите трябва да е между 1 и 100!")]
        public int TotalTables { get; set; }

        [Required(ErrorMessage = "Телефонният номер за контакт е задължителен!")]
        public string PhoneNumber { get; set; }

        //[Required(ErrorMessage = "Трябва да качите снимки.")]
        //[MinLength(3, ErrorMessage = "Трябва да качите 3 снимки.")]
        //[MaxLength(3, ErrorMessage = "Трябва да качите 3 снимки.")]
        public IBrowserFile[] Pictures { get; set; }
    }
}
