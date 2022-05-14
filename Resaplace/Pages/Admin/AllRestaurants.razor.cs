using Microsoft.AspNetCore.Components;
using Resaplace.Data.Models;
using Resaplace.Services;

namespace Resaplace.Pages.Admin
{
    public partial class AllRestaurants
    {
        [Inject]
        private RestaurantService RestaurantService { get; set; }

        private List<Restaurant> Restaurants { get; set; } = new List<Restaurant>();

        protected override async Task OnInitializedAsync()
        {
            Restaurants = await RestaurantService.GetAllRestaurantsAsync();
        }
    }
}
