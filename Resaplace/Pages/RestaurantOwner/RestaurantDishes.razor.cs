using Blazored.Toast.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Identity;
using Resaplace.Data.Models;
using Resaplace.Services;

namespace Resaplace.Pages.RestaurantOwner
{
    public partial class RestaurantDishes : ComponentBase
    {
        [Parameter]
        public int Id { get; set; }

        [CascadingParameter]
        private Task<AuthenticationState> AuthenticationStateTask { get; set; }
        [Inject]
        private RestaurantService RestaurantService { get; set; }
        [Inject]
        private DishService DishService { get; set; }
        [Inject]
        private UserManager<IdentityUser> UserManager { get; set; }
        [Inject]
        private NavigationManager NavManager { get; set; }
        [Inject]
        private OwnershipCheckService CheckService { get; set; }
        [Inject]
        private IToastService ToastService { get; set; }

        private Restaurant CurrentRestaurant { get; set; } = new Restaurant();
        private List<Dish> Dishes { get; set; } = new List<Dish>();
        private bool DeletePopup { get; set; } = false;
        private Dish DishToBeDeleted { get; set; }

        protected override async Task OnInitializedAsync()
        {
            var authState = await AuthenticationStateTask;
            var user = authState.User;
            IdentityUser idUser = await UserManager.FindByNameAsync(user.Identity.Name);

            bool userIsOwner = await CheckService.IsRestaurantOwnedByUser(Id, idUser);
            if (!userIsOwner) NavManager.NavigateTo("/myrestaurants");

            CurrentRestaurant = await RestaurantService.GetRestaurantByIdAsync(Id);
            Dishes = await DishService.GetDishesByRestaurantIdAsync(Id);
        }

        private void ShowDeletePopup(Dish dish)
        {
            DishToBeDeleted = dish;
            DeletePopup = true;
        }

        private void CancelDeletion()
        {
            DishToBeDeleted = null;
            DeletePopup = false;
        }

        private async Task AcceptDeletion()
        {
            await DishService.RemoveDishAsync(DishToBeDeleted);
            ToastService.ShowInfo($"Ястие {DishToBeDeleted.Name} е изтрито успешно!");
            DeletePopup = false;
            DishToBeDeleted = null;
            Dishes = await DishService.GetDishesByRestaurantIdAsync(Id);
        }
    }
}
