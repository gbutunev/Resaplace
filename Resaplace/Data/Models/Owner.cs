
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace Resaplace.Data.Models
{
    public class Owner
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string FirstName { get; set; }

        public string MiddleName { get; set; }
        [Required]
        public string LastName { get; set; }
        [Required]
        public string EIK { get; set; }
        [Required]
        public string CompanyName { get; set; }
        [Required]
        public string City { get; set; }
        [Required]
        public string Municipality { get; set; }
        [Required]
        public string Region { get; set; }
        [Required]
        public IdentityUser User { get; set; }

    }
}
