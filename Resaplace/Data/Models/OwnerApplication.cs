using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Resaplace.Data.Models
{
    //Not inherited from <Owner> class because it merges this table with ownerstable
    public class OwnerApplication
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
        public string Address { get; set; }
        [Required]
        public IdentityUser User { get; set; }
        [Required]
        public BasicStatus ApplicationStatus { get; set; }
        [Required]
        public DateTime CreatedOn { get; set; }
    }
}
