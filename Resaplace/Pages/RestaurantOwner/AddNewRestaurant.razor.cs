﻿using Blazored.Toast.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Identity;
using Resaplace.Data.Models;
using Resaplace.Services;
using Microsoft.AspNetCore.Hosting;

namespace Resaplace.Pages.RestaurantOwner
{
    public partial class AddNewRestaurant : ComponentBase
    {
        [CascadingParameter]
        private Task<AuthenticationState> AuthenticationStateTask { get; set; }
        [Inject]
        private IWebHostEnvironment Env { get; set; }
        [Inject]
        private RestaurantApplicationsService ResApplicationService { get; set; }
        [Inject]
        private UserManager<IdentityUser> UserManager { get; set; }
        [Inject]
        private IToastService ToastService { get; set; }
        [Inject]
        private NavigationManager NavManager { get; set; }
        private FormRestaurantApplication Model { get; set; } = new FormRestaurantApplication();
        private List<IBrowserFile> ModelFiles { get; set; } = new List<IBrowserFile>();
        private bool BoolClearInputFile { get; set; } = true;

        private async Task SubmitData()
        {
            var authState = await AuthenticationStateTask;
            var user = authState.User;

            if (ModelFiles.Count != 3)
            {
                ToastService.ShowError("Моля, качете 3 снимки на ресторанта!");
                return;
            }

            IdentityUser idUser = await UserManager.FindByNameAsync(user.Identity.Name);
            List<Image> images = new();
            try
            {
                foreach (IBrowserFile file in ModelFiles)
                {
                    var fileExtension = Utils.GetFileExtension(file);
                    var randomFileName = Guid.NewGuid().ToString() + fileExtension;
                    var path = Path.Combine(Env.WebRootPath, "images", randomFileName);

                    await using FileStream fs = new(path, FileMode.Create);
                    await file.OpenReadStream(5120000).CopyToAsync(fs);

                    images.Add(new Image() { ImagePath = randomFileName, AltText = $"Restaurant image {images.Count + 1}" });
                }
            }
            catch (Exception)
            {
                ToastService.ShowError("Имаше проблем с качването на снимките. Моля, опитайте по-късно.");
                return;
            }

            RestaurantApplication restaurantApplication = new(Model, idUser, images, BasicStatus.Pending);


            await ResApplicationService.InsertRestaurantApplicationAsync(restaurantApplication);
            ToastService.ShowSuccess($"Заявката за {restaurantApplication.Name} е подадена успешно!");
            NavManager.NavigateTo("/myrestaurantapplications");
        }

        private void LoadFiles(InputFileChangeEventArgs e)
        {
            ModelFiles.Clear();

            if (e.FileCount == 3)
            {
                IReadOnlyList<IBrowserFile> files = e.GetMultipleFiles(3);

                foreach (IBrowserFile file in files)
                {
                    if (file.ContentType == "image/png" || file.ContentType == "image/jpeg")
                    {
                        if (file.Size <= 5120000L)
                        {
                            ModelFiles.Add(file);
                        }
                        else
                        {
                            ClearInputFile();
                            ModelFiles.Clear();
                            ToastService.ShowError("Размерът на снимка не трябва да надхвърля 5MB!");
                            break;
                        }
                    }
                    else
                    {
                        ClearInputFile();
                        ModelFiles.Clear();
                        ToastService.ShowError("Снимките трябва да са във формат PNG или JPEG!");
                        break;
                    }
                }
            }
            else
            {
                ClearInputFile();
                ModelFiles.Clear();
                ToastService.ShowError("Трябва да бъдат качени 3 снимки!");
            }
        }
        //TODO: Check for duplicate names
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
