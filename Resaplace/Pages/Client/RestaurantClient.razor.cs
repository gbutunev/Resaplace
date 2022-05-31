using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Resaplace.Data.Models;
using Resaplace.Services;

namespace Resaplace.Pages.Client
{
    public partial class RestaurantClient : ComponentBase
    {
        #region Params
        [Parameter]
        public int Id { get; set; }

        [CascadingParameter]
        private Task<AuthenticationState> AuthenticationStateTask { get; set; }
        #endregion

        #region My services
        [Inject]
        private RestaurantService RestaurantService { get; set; }
        [Inject]
        private ReservationManagerService ReservationManager { get; set; }
        #endregion

        #region General Services
        [Inject]
        private NavigationManager NavigationManager { get; set; }
        #endregion

        private Restaurant CurrentRestaurant { get; set; }
        private bool LoggedIn { get; set; }
        private ReservationSubmition NewRes { get; set; } = new ReservationSubmition();

        private bool ShowPeopleNum { get; set; } = false;
        private bool ShowTime { get; set; } = false;
        private bool ShowDishMenu { get; set; } = false;
        private readonly string MinDate = DateTime.Now.ToString("yyyy-MM-dd");
        private readonly string MaxDate = DateTime.Now.AddMonths(2).ToString("yyyy-MM-dd");
        private bool DateDisabled = false;
        private bool PplNumDisabled = false;
        private DateOnly SelectedDate { get; set; } = new DateOnly();
        private int SelectedPplNum { get; set; } = 0;
        private List<TimeOnly> AvailableHours = null;
        private TimeOnly SelectedHour;

        protected override async Task OnParametersSetAsync()
        {
            CurrentRestaurant = await RestaurantService.GetRestaurantByIdAsync(Id);
            if (CurrentRestaurant == null) NavigationManager.NavigateTo("/");

            var authState = await AuthenticationStateTask;
            var user = authState.User;
            LoggedIn = user.Identity?.IsAuthenticated ?? false;
        }

        private void OnDateChange(ChangeEventArgs e)
        {
            SelectedDate = DateOnly.ParseExact(e.Value.ToString(), "yyyy-MM-dd", System.Globalization.CultureInfo.InvariantCulture);
            DateDisabled = true;
            ShowPeopleNum = true;
        }

        private void OnPplNumChange(ChangeEventArgs e)
        {
            SelectedPplNum = int.Parse(e.Value.ToString());
            PplNumDisabled = true;
            ShowTime = true;

            AvailableHours = ReservationManager.GetAvailableHours(Id, SelectedDate, SelectedPplNum);
            NewRes.GuestNumber = SelectedPplNum;
        }

        private void ChangeSelectedHour(TimeOnly t)
        {
            SelectedHour = t;
        }

        private void CreateReservation()
        {
            Console.WriteLine(CurrentRestaurant.Name);
            Console.WriteLine(SelectedDate);
            Console.WriteLine(SelectedPplNum);
            Console.WriteLine(SelectedHour);
        }

        private void ShowDishesComponent()
        {
            ShowDishMenu = true;
        }
    }
}
