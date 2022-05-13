using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace Resaplace.Data.Models
{
    public class RestaurantApplication
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public IdentityUser Owner { get; set; }
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
        public List<Image> Images { get; set; }
        public BasicStatus ApplicationStatus { get; set; }
        public string FeedbackMessage { get; set; }

        public RestaurantApplication() { }
        public RestaurantApplication(FormRestaurantApplication model, IdentityUser idUser, List<Image> images, BasicStatus status)
        {
            Images = images;
            Owner = idUser;
            Name = model.Name;
            Description = model.Description;
            Country = model.Country;
            City = model.City;
            StreetAddress = model.StreetAddress;
            TotalSeats = model.TotalSeats;
            TotalTables = model.TotalTables;
            PhoneNumber = model.PhoneNumber;
            ApplicationStatus = status;
        }
    }
}
