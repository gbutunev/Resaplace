using Microsoft.AspNetCore.Components;
using Resaplace.Data.Models;
using Resaplace.Services;

namespace Resaplace.Pages.Admin
{
    public partial class OwnerApplicationsArchive : ComponentBase
    {
        [Inject]
        private OwnerApplicationService ApplicationService { get; set; }
        private List<OwnerApplication> OldApplications { get; set; } = new List<OwnerApplication>();
        
        protected override async Task OnInitializedAsync()
        {
            OldApplications = await ApplicationService.GetApplicationsByStatusAsync(BasicStatus.Declined, true);
        }
    }
}
