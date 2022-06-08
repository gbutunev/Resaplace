using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace Resaplace.Data.Models
{
    public class FormOwnerApplication
    {
        [Required(ErrorMessage = "Името е задължително!")]
        public string FirstName { get; set; }

        public string MiddleName { get; set; }
        [Required(ErrorMessage = "Фамилията е задължителна!")]
        public string LastName { get; set; }
        [Required(ErrorMessage = "ЕИК е задължително!")]
        public string EIK { get; set; }
        [Required(ErrorMessage = "Наименованието е задължително!")]
        public string CompanyName { get; set; }
        [Required(ErrorMessage = "Градът е задължителен!")]
        public string City { get; set; }
        [Required(ErrorMessage = "Общината е задължителна!")]
        public string Municipality { get; set; }
        [Required(ErrorMessage = "Областта е задължителна!")]
        public string Region { get; set; }
        [Required(ErrorMessage = "Адресът е задължителен!")]
        public string Address { get; set; }
    }
}
