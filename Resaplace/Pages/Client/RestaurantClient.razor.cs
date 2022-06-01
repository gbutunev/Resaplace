using Blazored.Toast.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Identity;
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
        [Inject]
        private ReservationService ReservationService { get; set; }
        #endregion

        #region General Services
        [Inject]
        private NavigationManager NavigationManager { get; set; }
        [Inject]
        private UserManager<IdentityUser> UserManager { get; set; }
        [Inject]
        private IToastService ToastService { get; set; }
        #endregion

        private Restaurant CurrentRestaurant { get; set; }
        private bool LoggedIn { get; set; }
        private Reservation NewRes { get; set; } = new Reservation();
        private Dictionary<Dish, int> AddedDishes { get; set; } = new Dictionary<Dish, int>();
        private double TotalDishPrice
        {
            get
            {
                double sum = 0;
                foreach (var kv in AddedDishes)
                {
                    sum += kv.Value * kv.Key.Price;
                }

                return sum;
            }
        }

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
            NewRes.PeopleNumber = SelectedPplNum;
        }

        private void ChangeSelectedHour(TimeOnly t)
        {
            SelectedHour = t;
        }

        private async Task CreateReservation()
        {
            var authState = await AuthenticationStateTask;
            var user = authState.User;
            IdentityUser idUser = await UserManager.FindByNameAsync(user.Identity.Name);

            NewRes.User = idUser;
            NewRes.DateTime = new(SelectedDate.Year, SelectedDate.Month, SelectedDate.Day, SelectedHour.Hour, SelectedHour.Minute, 0);
            NewRes.PeopleNumber = SelectedPplNum;
            NewRes.Dishes = new List<Dish>();
            NewRes.Restaurant = CurrentRestaurant;
            //TODO: NewRes.Message 

            foreach (var kv in AddedDishes)
            {
                for (int i = 0; i < kv.Value; i++)
                {
                    NewRes.Dishes.Add(kv.Key);
                }
            }

            if (await ReservationService.InsertReservationAsync(NewRes))
            {
                ToastService.ShowSuccess("Резервацията е успешно създадена!");
                NavigationManager.NavigateTo("/");
            }
        }

        private void ShowDishesComponent() => ShowDishMenu = true;

        private void HideDishesComponent() => ShowDishMenu = false;
    }
}
