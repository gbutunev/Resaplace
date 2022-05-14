using Microsoft.AspNetCore.Components;
using Resaplace.Data.Models;
using Resaplace.Services;

namespace Resaplace.Pages.Admin
{
    public partial class RestaurantApplicationsArchive : ComponentBase
    {
        #region SERVICES
        [Inject]
        private RestaurantApplicationsService ResApplicationService { get; set; }
        #endregion

        #region PROPERTIES
        private List<RestaurantApplication> Applications { get; set; } = new List<RestaurantApplication>();
        #endregion

        protected override async Task OnInitializedAsync()
        {
            Applications = await ResApplicationService.GetArchivedRestaurantApplicationAsync();
        }
    }
}
