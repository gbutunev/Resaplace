using Blazored.Toast.Services;
using Microsoft.AspNetCore.Components;
using Resaplace.Data.Models;
using Resaplace.Services;

namespace Resaplace.Pages.Admin
{
    public partial class OwnerApplications : ComponentBase
    {
        [Inject]
        private OwnerApplicationService ApplicationService { get; set; }
        [Inject]
        private IToastService ToastService { get; set; }
        [Inject]
        private OwnerService OwnerService { get; set; }
        private List<OwnerApplication> PendingApplications { get; set; } = new List<OwnerApplication>();
        private bool ShowAcceptPopup { get; set; } = false;
        private bool ShowDeclinePopup { get; set; } = false;
        private OwnerApplication CurrentApplication { get; set; } = null;

        protected override async Task OnInitializedAsync()
        {
            PendingApplications = await ApplicationService.GetApplicationsByStatusAsync(BasicStatus.Pending);
        }

        private void ShowAccept(OwnerApplication application)
        {
            ShowAcceptPopup = true;
            CurrentApplication = application;
        }

        private void ShowDecline(OwnerApplication application)
        {
            ShowDeclinePopup = true;
            CurrentApplication = application;
        }

        private void CancelAccept()
        {
            ShowAcceptPopup = false;
            CurrentApplication = null;
        }

        private void CancelDecline()
        {
            ShowDeclinePopup = false;
            CurrentApplication = null;
        }

        private async Task AcceptDecline()
        {
            await ApplicationService.UpdateApplicationStatusAsync(CurrentApplication, BasicStatus.Declined);
            ShowDeclinePopup = false;
            CurrentApplication = null;
            ToastService.ShowInfo("Кандидатурата е отказана!");
            PendingApplications = await ApplicationService.GetApplicationsByStatusAsync(BasicStatus.Pending);
        }

        private async Task AcceptAccept()
        {
            //1.update application status
            //2.add user to owners table
            //3.change user role

            await ApplicationService.UpdateApplicationStatusAsync(CurrentApplication, BasicStatus.Accepted);

            Owner newOwner = new Owner
            {
                FirstName = CurrentApplication.FirstName,
                MiddleName = CurrentApplication.MiddleName,
                LastName = CurrentApplication.LastName,
                Address = CurrentApplication.Address,
                City = CurrentApplication.City,
                Municipality = CurrentApplication.Municipality,
                Region = CurrentApplication.Region,
                CompanyName = CurrentApplication.CompanyName,
                EIK = CurrentApplication.EIK,
                User = CurrentApplication.User
            };

            bool exists = await OwnerService.UserExists(newOwner);
            if (exists)
            {
                ToastService.ShowError("Потребителят вече съществува като собственик! По принцип това не трябва да става, но ако стане, ще го оправя после.");
            }
            else
            {
                await OwnerService.AddOwner(newOwner);
            }

            ShowAcceptPopup = false;
            CurrentApplication = null;
            PendingApplications = await ApplicationService.GetApplicationsByStatusAsync(BasicStatus.Pending);
        }
    }
}
