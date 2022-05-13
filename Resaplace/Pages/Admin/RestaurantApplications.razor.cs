using Microsoft.AspNetCore.Components;
using Resaplace.Data.Models;
using Resaplace.Services;

namespace Resaplace.Pages.Admin
{
    public partial class RestaurantApplications : ComponentBase
    {
        [Inject]
        private RestaurantApplicationsService ResApplicationService { get; set; }


        private List<RestaurantApplication> Applications { get; set; }

        protected override async Task OnInitializedAsync()
        {
            Applications = await ResApplicationService.GetAllRestaurantApplicationsAsync();
        }
    }
}
