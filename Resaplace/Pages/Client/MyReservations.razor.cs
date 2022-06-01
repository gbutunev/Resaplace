using Blazored.Toast.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Identity;
using Resaplace.Data.Models;
using Resaplace.Services;

namespace Resaplace.Pages.Client
{
    public partial class MyReservations : ComponentBase
    {
        [CascadingParameter]
        private Task<AuthenticationState> AuthenticationStateTask { get; set; }
        [Inject]
        private UserManager<IdentityUser> UserManager { get; set; }
        [Inject]
        private NavigationManager NavManager { get; set; }
        [Inject]
        private IToastService ToastService { get; set; }
        [Inject]
        private ReservationService ReservationService { get; set; }

        private List<Reservation> Reservations { get; set; } = new List<Reservation>();
        private bool DeleteReservationPopup { get; set; } = false;
        private Reservation ReservationToBeDeleted { get; set; }
        private IdentityUser IdUser { get; set; }

        protected override async Task OnInitializedAsync()
        {
            var authState = await AuthenticationStateTask;
            var user = authState.User;
            IdUser = await UserManager.FindByNameAsync(user.Identity.Name);

            Reservations = await ReservationService.GetReservationsByUser(IdUser);
        }

        private void ShowDeletePopup(Reservation res)
        {
            ReservationToBeDeleted = res;
            DeleteReservationPopup = true;
        }

        private void CancelDeletion()
        {
            ReservationToBeDeleted = null;
            DeleteReservationPopup = false;
        }

        private async Task AcceptDeletion()
        {
            ReservationService.DeleteReservation(ReservationToBeDeleted);
            ToastService.ShowInfo($"Резервацията в {ReservationToBeDeleted.Restaurant.Name} е изтрита успешно!");
            DeleteReservationPopup = false;
            ReservationToBeDeleted = null;
            Reservations = await ReservationService.GetReservationsByUser(IdUser);
        }
    }
}
