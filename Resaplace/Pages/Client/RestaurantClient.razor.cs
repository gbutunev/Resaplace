using Microsoft.AspNetCore.Components;
using Resaplace.Data.Models;
using Resaplace.Services;

namespace Resaplace.Pages.Client
{
    public partial class RestaurantClient : ComponentBase
    {
        [Parameter]
        public int Id { get; set; }

        [Inject]
        private RestaurantService RestaurantService { get; set; }

        private Restaurant CurrentRestaurant { get; set; }

        protected override async Task OnParametersSetAsync()
        {
            CurrentRestaurant = await RestaurantService.GetRestaurantByIdAsync(Id);
        }
    }
}
