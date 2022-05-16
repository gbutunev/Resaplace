using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Identity;
using Resaplace.Data.Models;
using Resaplace.Services;

namespace Resaplace.Pages.RestaurantOwner
{
    public partial class MyApplications : ComponentBase
    {
        [CascadingParameter]
        private Task<AuthenticationState> authenticationStateTask { get; set; }
        [Inject]
        private UserManager<IdentityUser> UserManager { get; set; }
        [Inject]
        private RestaurantApplicationsService ApplicationService { get; set; }

        private List<RestaurantApplication> CurrentApplications { get; set; } = new List<RestaurantApplication>();
        private List<RestaurantApplication> ArchivedApplications { get; set; } = new List<RestaurantApplication>();

        protected override async Task OnInitializedAsync()
        {
            var authState = await authenticationStateTask;
            var user = authState.User;
            IdentityUser idUser = await UserManager.FindByNameAsync(user.Identity.Name);

            CurrentApplications = await ApplicationService.GetPendingRestaurantApplicationsByOwnerAsync(idUser);
            ArchivedApplications = await ApplicationService.GetArchivedRestaurantApplicationsByOwnerAsync(idUser);
        }
    }
}
