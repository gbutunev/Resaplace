using Microsoft.AspNetCore.Components;
using Resaplace.Data.Models;
using Resaplace.Services;

namespace Resaplace.Pages.Components
{
    public partial class DishesModal : ComponentBase
    {
        [Parameter]
        public int RestaurantId { get; set; }
        [Parameter]
        public Action CloseModal { get; set; }
        [Parameter]
        public ReservationSubmition Reservation { get; set; }
        [Parameter]
        public Dictionary<Dish, int> AddedDishes { get; set; }

        [Inject]
        private DishService DishService { get; set; }

        private List<Dish> AllDishes { get; set; } = new List<Dish>();

        protected override async Task OnParametersSetAsync()
        {
            AllDishes = await DishService.GetDishesByRestaurantIdAsync(RestaurantId);
        }

        private void DecreaseDishCount(Dish d)
        {
            if (!AddedDishes.ContainsKey(d)) return;
            else if (AddedDishes[d] == 1) AddedDishes.Remove(d);
            else AddedDishes[d]--;
        }

        private void IncreaseDishCount(Dish d)
        {
            if (AddedDishes.ContainsKey(d))
            {
                if (AddedDishes[d] < Reservation.GuestNumber) AddedDishes[d]++;
            }
            else
            {
                AddedDishes.Add(d, 1);
            }
        }
    }
}
