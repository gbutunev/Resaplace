using Blazored.Toast.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Identity;
using Resaplace.Data.Models;
using Resaplace.Services;

namespace Resaplace.Pages.Admin
{
    public partial class AllRestaurants
    {
        [Inject]
        private RestaurantService RestaurantService { get; set; }
        [Inject]
        private OwnerService OwnerService { get; set; }
        [Inject]
        private IToastService ToastService { get; set; }

        private List<Restaurant> Restaurants { get; set; } = new List<Restaurant>();
        private Restaurant RestaurantToBeDeleted { get; set; } = new Restaurant();
        private Owner CurrentOwner { get; set; } = null;


        private bool DeleteRestaurantPopup { get; set; } = false;
        private bool ShowOwnerInfoPopup { get; set; } = false;
        protected override async Task OnInitializedAsync()
        {
            Restaurants = await RestaurantService.GetAllRestaurantsAsync();
        }

        private void ShowDeletePopup(Restaurant restaurant)
        {
            RestaurantToBeDeleted = restaurant;
            DeleteRestaurantPopup = true;
        }

        private void CancelDeletion()
        {
            RestaurantToBeDeleted = null;
            DeleteRestaurantPopup = false;
        }

        private async Task AcceptDeletion()
        {
            RestaurantService.DeleteRestaurant(RestaurantToBeDeleted);
            ToastService.ShowInfo($"Ресторант {RestaurantToBeDeleted.Name} е изтрит успешно!");
            DeleteRestaurantPopup = false;
            RestaurantToBeDeleted = new Restaurant();
            Restaurants = await RestaurantService.GetAllRestaurantsAsync();
        }

        private async Task ShowOwnerInfo(IdentityUser user)
        {
            CurrentOwner = await OwnerService.GetOwnerInfo(user);
            if (CurrentOwner == null)
                ToastService.ShowError("Грешка");
            else
                ShowOwnerInfoPopup = true;
        }
        private void HideOwnerInfo()
        {
            ShowOwnerInfoPopup = false;
            CurrentOwner = null;
        }
    }
}
