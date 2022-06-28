using Blazored.Toast.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Identity;
using Resaplace.Data.Models;
using Resaplace.Services;
using RestSharp;
using System.Text;

namespace Resaplace.Pages.Client
{
    public partial class RestaurantOwnerForm : ComponentBase
    {
        //private readonly string _trBaseUri = "https://sova.bg/";
        //private readonly string _trSearchUri = "api/v1/com/search/search_input.do";
        //private readonly string _trSearchBodyRaw = @"{""api_ver"":0.057,""timeout"":652476588,""params"":{""term"":""121699202""}}";
        //[Inject]
        //private IHttpClientFactory ClientFactory { get; set; }
        //private HttpClient Client { get; set; }
        [CascadingParameter]
        private Task<AuthenticationState> AuthenticationStateTask { get; set; }

        [Inject]
        private IToastService ToastService { get; set; }
        [Inject]
        private OwnerApplicationService ApplicationService { get; set; }
        [Inject]
        private UserManager<IdentityUser> UserManager { get; set; }
        [Inject]
        private NavigationManager NavigationManager { get; set; }

        private FormOwnerApplication Model { get; set; } = new FormOwnerApplication();
        private bool canApply = true;
        private BasicStatus pageStatus;
        private IdentityUser CurrentUser { get; set; }
        protected override async Task OnInitializedAsync()
        {
            //Client = ClientFactory.CreateClient();

            var authState = await AuthenticationStateTask;
            var user = authState.User;
            CurrentUser = await UserManager.FindByNameAsync(user.Identity.Name);

            var lastApplication = await ApplicationService.GetLastApplicationByUserAsync(CurrentUser);
            if (lastApplication == null)
            {
                pageStatus = BasicStatus.Declined;
            }
            else
            {
                pageStatus = lastApplication.ApplicationStatus;
            }
        }

        //private async Task CheckEik()
        //{
        //    if (string.IsNullOrWhiteSpace(Model.EIK)) return;

        //    var client = new RestClient("https://sova.bg/");
        //    var request = new RestRequest("api/v1/com/search/search_input.do", Method.Post);
        //    request.AddHeader("Content-Type", "application/json");
        //    var body = $"{{\"api_ver\": 0.057,\"timeout\": 652476588,\"params\": {{\"term\": \"{Model.EIK.Trim()}\"}}}}";
        //    request.AddParameter("application/json", body, ParameterType.RequestBody);
        //    RestResponse? response = client.Execute(request);

        //    ToastService.ShowInfo(response.Content);
        //}

        private async Task ShowConfirmation()
        {
            OwnerApplication newApplication = new OwnerApplication
            {
                FirstName = Model.FirstName.Trim(),
                MiddleName = Model.MiddleName == null ? String.Empty : Model.MiddleName.Trim(),
                LastName = Model.LastName.Trim(),
                Address = Model.Address.Trim(),
                City = Model.City.Trim(),
                Municipality = Model.Municipality.Trim(),
                Region = Model.Region.Trim(),
                CompanyName = Model.CompanyName.Trim(),
                EIK = Model.EIK.Trim(),
                User = CurrentUser,
                CreatedOn = DateTime.Now,
                ApplicationStatus = BasicStatus.Pending
            };

            await ApplicationService.CreateApplicationAsync(newApplication);

            Model = new FormOwnerApplication();
            ToastService.ShowSuccess("Вашата заявка беше изпратена!");
            NavigationManager.NavigateTo("/");
        }
    }
}
