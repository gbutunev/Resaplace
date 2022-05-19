using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Identity;
using Resaplace.Data.Models;
using Resaplace.Services;

namespace Resaplace.Pages.RestaurantOwner
{
    public partial class MyRestaurants
    {
        [CascadingParameter]
        private Task<AuthenticationState> AuthenticationStateTask { get; set; }
        [Inject]
        private RestaurantService RestaurantService { get; set; }
        [Inject]
        private UserManager<IdentityUser> UserManager { get; set; }
        [Inject]
        private NavigationManager NavManager { get; set; }

        private List<Restaurant> Restaurants { get; set; } = new List<Restaurant>();

        protected override async Task OnInitializedAsync()
        {
            var authState = await AuthenticationStateTask;
            var user = authState.User;
            IdentityUser idUser = await UserManager.FindByNameAsync(user.Identity.Name);

            Restaurants = await RestaurantService.GetRestaurantsByOwner(idUser);
        }

        private void NavigateToRestaurant(int restaurantId)
        {
            NavManager.NavigateTo($"/myrestaurants/{restaurantId}/edit/");
        }
        
        private void NavigateToDishes(int restaurantId)
        {
            NavManager.NavigateTo($"/myrestaurants/{restaurantId}/dishes/");
        }
    }
}
