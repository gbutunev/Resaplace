using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Identity;
using Resaplace.Data.Models;
using Resaplace.Services;

namespace Resaplace.Pages.Staff
{
    public partial class MainDashboard : ComponentBase
    {
        [CascadingParameter]
        private Task<AuthenticationState> AuthenticationStateTask { get; set; }

        [Inject]
        private UserManager<IdentityUser> UserManager { get; set; }
        [Inject]
        private RestaurantStaffService StaffService { get; set; }

        private List<Restaurant> Restaurants { get; set; } = new List<Restaurant>();

        protected override async Task OnInitializedAsync()
        {

            var authState = await AuthenticationStateTask;
            var user = authState.User;
            IdentityUser idUser = await UserManager.FindByNameAsync(user.Identity.Name);

            Restaurants = await StaffService.GetRestaurantsByStaffMemberAsync(idUser);
        }
    }
}
