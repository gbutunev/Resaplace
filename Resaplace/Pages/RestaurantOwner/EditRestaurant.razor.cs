using Blazored.Toast.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Identity;
using Resaplace.Data.Models;
using Resaplace.Services;

namespace Resaplace.Pages.RestaurantOwner
{
    public partial class EditRestaurant : ComponentBase
    {
        [Parameter]
        public int Id { get; set; }

        [CascadingParameter]
        private Task<AuthenticationState> AuthenticationStateTask { get; set; }
        [Inject]
        private RestaurantService RestaurantService { get; set; }
        [Inject]
        private OwnershipCheckService CheckingService { get; set; }
        [Inject]
        private UserManager<IdentityUser> UserManager { get; set; }
        [Inject]
        private NavigationManager NavManager { get; set; }
        [Inject]
        private IToastService ToastService { get; set; }

        private Restaurant RestaurantToEdit { get; set; } = new Restaurant();
        private FormEditRestaurant Model { get; set; } = new FormEditRestaurant();

        private bool EditRestaurantPopup { get; set; } = false;

        protected override async Task OnInitializedAsync()
        {
            var authState = await AuthenticationStateTask;
            var user = authState.User;
            IdentityUser idUser = await UserManager.FindByNameAsync(user.Identity.Name);

            bool userIsOwner = await CheckingService.IsRestaurantOwnedByUser(Id, idUser);
            if (!userIsOwner) NavManager.NavigateTo("/myrestaurants");
            else
            {
                RestaurantToEdit = await RestaurantService.GetRestaurantByIdAsync(Id);

                Model.Name = RestaurantToEdit.Name;
                Model.Description = RestaurantToEdit.Description;
                Model.Country = RestaurantToEdit.Country;
                Model.City = RestaurantToEdit.City;
                Model.StreetAddress = RestaurantToEdit.StreetAddress;
                Model.PhoneNumber = RestaurantToEdit.PhoneNumber;
                Model.TotalSeats = RestaurantToEdit.TotalSeats;
                Model.TotalTables = RestaurantToEdit.TotalTables;
            }
        }

        private void OnSubmit()
        {
            EditRestaurantPopup = true;
        }

        private void CancelEdit()
        {
            EditRestaurantPopup = false;
        }

        private async Task AcceptEdit()
        {
            RestaurantToEdit.Name = Model.Name;
            RestaurantToEdit.Description = Model.Description;
            RestaurantToEdit.Country = Model.Country;
            RestaurantToEdit.City = Model.City;
            RestaurantToEdit.StreetAddress = Model.StreetAddress;
            RestaurantToEdit.PhoneNumber = Model.PhoneNumber;
            RestaurantToEdit.TotalSeats = Model.TotalSeats;
            RestaurantToEdit.TotalTables = Model.TotalTables;

            await RestaurantService.UpdateRestaurantAsync(RestaurantToEdit);
            EditRestaurantPopup = false;
            ToastService.ShowSuccess("Ресторантът е променен успешно!");
            NavManager.NavigateTo("/myrestaurants");
        }
    }
}
