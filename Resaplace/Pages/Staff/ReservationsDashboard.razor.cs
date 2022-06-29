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
        [Inject]
        private ReservationService ReservationService { get; set; }

        private Restaurant CurrentRestaurant { get; set; } = new Restaurant();
        private List<Reservation> ReservationsToday { get; set; } = new List<Reservation>();
        private List<Reservation> PendingReservations { get; set; } = new List<Reservation>();
        private List<Reservation> PastReservations { get; set; } = new List<Reservation>();
        private Reservation SelectedReservation { get; set; } = null;
        private bool ShowAcceptPopup { get; set; }
        private bool ShowDeclinePopup { get; set; }

        protected override async Task OnParametersSetAsync()
        {
            var authState = await AuthenticationStateTask;
            var user = authState.User;
            IdentityUser idUser = await UserManager.FindByNameAsync(user.Identity.Name);
            CurrentRestaurant = await RestaurantService.GetRestaurantByIdAsync(Id);

            bool userIsStaff = await StaffService.ContainsStaffMember(idUser, CurrentRestaurant);
            if (!userIsStaff) NavManager.NavigateTo("/");
            await UpdateLists();
        }

        private async Task UpdateLists()
        {
            ReservationsToday = await ReservationService
                            .GetReservationsByRestaurantAndDate(CurrentRestaurant, DateTime.Today);

            PendingReservations = ReservationsToday.Where(x => x.ReservationStatus == BasicStatus.Pending).ToList();
            PastReservations = ReservationsToday.Where(x => x.ReservationStatus != BasicStatus.Pending).ToList();
        }

        private void ShowAccept(Reservation r)
        {
            SelectedReservation = r;
            ShowAcceptPopup = true;
        }

        private void ShowDecline(Reservation r)
        {
            SelectedReservation = r;
            ShowDeclinePopup = true;
        }

        private void CancelAccept()
        {
            SelectedReservation = null;
            ShowAcceptPopup = false;
        }

        private void CancelDecline()
        {
            SelectedReservation = null;
            ShowDeclinePopup = false;
        }

        private async Task AcceptAccept()
        {
            await ReservationService.SetReservationStatus(SelectedReservation, BasicStatus.Accepted);
            SelectedReservation = null;
            ShowAcceptPopup = false;
            await UpdateLists();
            StateHasChanged();
        }

        private async Task AcceptDecline()
        {
            await ReservationService.SetReservationStatus(SelectedReservation, BasicStatus.Declined);
            SelectedReservation = null;
            ShowDeclinePopup = false;
            await UpdateLists();
            StateHasChanged();
        }
    }
}
