using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Identity;
using Resaplace.Data.Models;
using Resaplace.Services;

namespace Resaplace.Pages.Staff
{
    public partial class ReservationsDashboard : ComponentBase
    {
        [Parameter]
        public int Id { get; set; }

        [CascadingParameter]
        private Task<AuthenticationState> AuthenticationStateTask { get; set; }

        [Inject]
        private UserManager<IdentityUser> UserManager { get; set; }
        [Inject]
        private RestaurantStaffService StaffService { get; set; }
        [Inject]
        private RestaurantService RestaurantService { get; set; }
        [Inject]
        private NavigationManager NavManager { get; set; }

        private Restaurant CurrentRestaurant { get; set; } = null;

        protected override async Task OnParametersSetAsync()
        {
            var authState = await AuthenticationStateTask;
            var user = authState.User;
            IdentityUser idUser = await UserManager.FindByNameAsync(user.Identity.Name);
            CurrentRestaurant = await RestaurantService.GetRestaurantByIdAsync(Id);

            bool userIsStaff = await StaffService.ContainsStaffMember(idUser, CurrentRestaurant);
            if (!userIsStaff) NavManager.NavigateTo("/");
        }
    }
}
