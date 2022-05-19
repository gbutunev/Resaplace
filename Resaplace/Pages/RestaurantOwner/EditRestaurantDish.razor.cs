using Blazored.Toast.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Identity;
using Resaplace.Data.Models;
using Resaplace.Services;

namespace Resaplace.Pages.RestaurantOwner
{
    public partial class EditRestaurantDish : ComponentBase
    {
        [Parameter]
        public int RestaurantId { get; set; }
        [Parameter]
        public int DishId { get; set; }


        [CascadingParameter]
        private Task<AuthenticationState> AuthenticationStateTask { get; set; }

        [Inject]
        private UserManager<IdentityUser> UserManager { get; set; }
        [Inject]
        private OwnershipCheckService CheckService { get; set; }
        [Inject]
        private RestaurantService RestaurantService { get; set; }
        [Inject]
        private DishService DishService { get; set; }
        [Inject]
        private NavigationManager NavManager { get; set; }
        [Inject]
        private IToastService ToastService { get; set; }
        [Inject]
        private IWebHostEnvironment Env { get; set; }

        private Restaurant CurrentRestaurant { get; set; } = new Restaurant();
        private FormDish Model { get; set; } = new FormDish();
        private Dish CurrentDish { get; set; } = new Dish();
        private bool BoolClearInputFile { get; set; } = true;

        protected override async Task OnInitializedAsync()
        {
            var authState = await AuthenticationStateTask;
            var user = authState.User;
            IdentityUser idUser = await UserManager.FindByNameAsync(user.Identity.Name);

            bool userIsOwner = await CheckService.IsRestaurantOwnedByUser(RestaurantId, idUser);
            if (!userIsOwner) NavManager.NavigateTo("/myrestaurants");

            CurrentRestaurant = await RestaurantService.GetRestaurantByIdAsync(RestaurantId);
            if (!CurrentRestaurant.Dishes.Exists(x => x.Id == DishId)) NavManager.NavigateTo($"/myrestaurants/{RestaurantId}/dishes");

            CurrentDish = await DishService.GetDishByIdAsync(DishId);

            Model.Name = CurrentDish.Name;
            Model.Price = CurrentDish.Price;
        }

        private async Task SubmitData()
        {
            CurrentDish.Name = Model.Name;
            CurrentDish.Price = Model.Price;

            if (Model.Image == null)
            {
                await DishService.UpdateDishAsync(CurrentDish);
                ToastService.ShowSuccess("Продуктът е успешно променен!");
                NavManager.NavigateTo($"/myrestaurants/{RestaurantId}/dishes");
            }
            else
            {
                Image newImage;

                try
                {
                    var fileExtension = Utils.GetFileExtension(Model.Image);
                    var randomFileName = Guid.NewGuid().ToString() + fileExtension;
                    var path = Path.Combine(Env.WebRootPath, "images", randomFileName);

                    await using FileStream fs = new(path, FileMode.Create);
                    await Model.Image.OpenReadStream(5120000).CopyToAsync(fs);

                    newImage = new Image() { ImagePath = randomFileName, AltText = Model.Name };
                }
                catch (Exception)
                {
                    ToastService.ShowError("Имаше проблем с качването на снимката. Моля, опитайте по-късно.");
                    return;
                }

                CurrentDish.Image = newImage;

                await DishService.UpdateDishAsync(CurrentDish);
                ToastService.ShowSuccess("Продуктът е успешно променен!");
                NavManager.NavigateTo($"/myrestaurants/{RestaurantId}/dishes");
            }
        }

        private void LoadFiles(InputFileChangeEventArgs e)
        {
            Model.Image = null;

            IBrowserFile file = e.File;

            if (file.ContentType == "image/png" || file.ContentType == "image/jpeg")
            {
                if (file.Size <= 5120000L)
                {
                    Model.Image = file;
                }
                else
                {
                    ClearInputFile();
                    ToastService.ShowError("Размерът на снимка не трябва да надхвърля 5MB!");
                }
            }
            else
            {
                ClearInputFile();
                ToastService.ShowError("Снимките трябва да са във формат PNG или JPEG!");
            }
        }

        //Showing and hiding element clears the uploaded files
        //TODO: Find a better solution
        private void ClearInputFile()
        {
            BoolClearInputFile = false;
            StateHasChanged();
            BoolClearInputFile = true;
            StateHasChanged();
        }
    }
}
