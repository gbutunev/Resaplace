using Blazored.Toast.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Identity;
using Resaplace.Data.Models;
using Resaplace.Services;

namespace Resaplace.Pages.RestaurantOwner
{
    public partial class RestaurantStaff : ComponentBase
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
        private RestaurantStaffService StaffService { get; set; }
        [Inject]
        private NavigationManager NavManager { get; set; }
        [Inject]
        private IToastService ToastService { get; set; }

        private Restaurant CurrentRestaurant { get; set; } = new Restaurant();
        private List<IdentityUser> StaffMembers { get; set; } = new List<IdentityUser>();
        private IdentityUser StaffToBeDeleted { get; set; }

        private bool DeleteStaffPopup { get; set; } = false;
        private bool AddStaffPopup { get; set; } = false;
        private string InputField { get; set; } = string.Empty;

        protected override async Task OnParametersSetAsync()
        {
            var authState = await AuthenticationStateTask;
            var user = authState.User;
            IdentityUser idUser = await UserManager.FindByNameAsync(user.Identity.Name);

            bool userIsOwner = await CheckingService.IsRestaurantOwnedByUser(Id, idUser);
            if (!userIsOwner) NavManager.NavigateTo("/myrestaurants");

            CurrentRestaurant = await RestaurantService.GetRestaurantByIdAsync(Id);
            StaffMembers = await StaffService.GetStaffMembersByRestaurantAsync(CurrentRestaurant);
        }

        private void ShowAddStaffModal()
        {
            if (string.IsNullOrWhiteSpace(InputField))
            {
                ToastService.ShowError("Въведете имейл!");
                return;
            }
            AddStaffPopup = true;
        }
        private void HideStaffPopup() => AddStaffPopup = false;
        private async Task AddStaffMember()
        {
            InputField = InputField.Trim();

            IdentityUser user = await UserManager.FindByNameAsync(InputField);

            if (user == null)
            {
                ToastService.ShowError("Такъв потребител не съществува!");
                HideStaffPopup();
                return;
            }

            bool res = await StaffService.AddStaffMember(user, CurrentRestaurant);
            if (res)
            {
                ToastService.ShowSuccess("Служителят е добавен успешно!");
                StaffMembers = await StaffService.GetStaffMembersByRestaurantAsync(CurrentRestaurant);
            }
            else
            {
                ToastService.ShowWarning("Служителят вече е добавен!");
            }
            InputField = string.Empty;
            HideStaffPopup();
        }

        private void DeleteStaffMember(IdentityUser user)
        {
            DeleteStaffPopup = true;
            StaffToBeDeleted = user;
        }

        private void CancelDeletion()
        {
            DeleteStaffPopup = false;
            StaffToBeDeleted = null;
        }

        private async Task AcceptDeletion()
        {
            await StaffService.RemoveStaffMemberAsync(StaffToBeDeleted, CurrentRestaurant);
            DeleteStaffPopup = false;
            StaffToBeDeleted = null;

            ToastService.ShowInfo("Потребителят е изтрит от списъка със служители");
            StaffMembers = await StaffService.GetStaffMembersByRestaurantAsync(CurrentRestaurant);
        }
    }
}
