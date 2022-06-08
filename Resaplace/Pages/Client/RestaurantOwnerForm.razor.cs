using Blazored.Toast.Services;
using Microsoft.AspNetCore.Components;
using Resaplace.Data.Models;
using RestSharp;
using System.Text;

namespace Resaplace.Pages.Client
{
    public partial class RestaurantOwnerForm : ComponentBase
    {
        //private readonly string _trBaseUri = "https://sova.bg/";
        //private readonly string _trSearchUri = "api/v1/com/search/search_input.do";
        //private readonly string _trSearchBodyRaw = @"{""api_ver"":0.057,""timeout"":652476588,""params"":{""term"":""121699202""}}";
        [Inject]
        private IToastService ToastService { get; set; }
        [Inject]
        //private IHttpClientFactory ClientFactory { get; set; }

        private FormOwnerApplication Model { get; set; } = new FormOwnerApplication();
        //private HttpClient Client { get; set; }
        protected override async Task OnInitializedAsync()
        {
            //Client = ClientFactory.CreateClient();
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

        private void ShowConfirmation()
        {
            ToastService.ShowWarning(Model.EIK);
        }
    }
}
