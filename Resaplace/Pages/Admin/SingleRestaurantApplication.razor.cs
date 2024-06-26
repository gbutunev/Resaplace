﻿using Blazored.Toast.Services;
using Microsoft.AspNetCore.Components;
using Resaplace.Data.Models;
using Resaplace.Services;

namespace Resaplace.Pages.Admin
{
    public partial class SingleRestaurantApplication : ComponentBase
    {
        #region SERVICES
        [Inject]
        private RestaurantApplicationsService ResApplicationService { get; set; }
        [Inject]
        private RestaurantService RestaurantService { get; set; }
        [Inject]
        private IToastService ToastService { get; set; }
        [Inject]
        private NavigationManager NavigationManager { get; set; }
        [Inject]
        private OwnerService OwnerService { get; set; }
       #endregion

        #region PARAMETERS
        [Parameter]
        public int Id { get; set; }
        #endregion

        #region FUNCTIONAL PROPERTIES
        private RestaurantApplication CurrentApplication { get; set; }
        private Owner CurrentOwner { get; set; }
        private string InitialMessage { get; set; } = "Зареждане...";
        private string FeedbackMessage { get; set; } = string.Empty;
        #endregion

        #region VISUALS
        public bool ShowAcceptPopup { get; set; } = false;
        public bool ShowDeclinePopup { get; set; } = false;
        #endregion

        protected override async Task OnParametersSetAsync()
        {
            CurrentApplication = await ResApplicationService.GetRestaurantApplicationByIdAsync(Id);
            if (CurrentApplication.ApplicationStatus != BasicStatus.Pending) NavigationManager.NavigateTo("/restaurantapplications");
            if (CurrentApplication == null) InitialMessage = "Заявката не съществува.";
            CurrentOwner = await OwnerService.GetOwnerInfo(CurrentApplication.Owner);
        }

        private void ApproveRestaurantButtonClick() => ShowAcceptPopup = true;

        private void CancelApproval() => ShowAcceptPopup = false;

        private async Task AcceptApproval()
        {
            Restaurant newRestaurant = new()
            {
                Name = CurrentApplication.Name,
                Description = CurrentApplication.Description,
                Country = CurrentApplication.Country,
                City = CurrentApplication.City,
                StreetAddress = CurrentApplication.StreetAddress,
                PhoneNumber = CurrentApplication.PhoneNumber,
                TotalTables = CurrentApplication.TotalTables,
                TotalSeats = CurrentApplication.TotalSeats,
                Images = CurrentApplication.Images,
                Owner = CurrentApplication.Owner,
            };

            await RestaurantService.InsertRestaurantWithApplicationAsync(newRestaurant, CurrentApplication);

            ToastService.ShowSuccess("Ресторантът е успешно създаден!");
            NavigationManager.NavigateTo("/restaurantapplications");
        }

        private void DenyRestaurantButtonClick()
        {
            FeedbackMessage = string.Empty;
            ShowDeclinePopup = true;
        }

        private void CancelDenial()
        {
            FeedbackMessage = string.Empty;
            ShowDeclinePopup = false;
        }


        private async Task AcceptDenial()
        {
            if (string.IsNullOrWhiteSpace(FeedbackMessage))
            {
                ShowDeclinePopup = false;
                ToastService.ShowError("Трябва да въведете причина за отказ на заявката!");
                return;
            }

            await ResApplicationService.SetFeedbackMessageAsync(Id, FeedbackMessage);
            await ResApplicationService.ChangeRestaurantApplicationStatusAsync(Id, BasicStatus.Declined);
            ToastService.ShowInfo("Заявката е отказана!");
            ShowDeclinePopup = false;
        }
    }
}
