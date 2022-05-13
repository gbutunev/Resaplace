using Microsoft.AspNetCore.Components;
using Resaplace.Data.Models;

namespace Resaplace.Pages.Admin
{
    public partial class RestaurantApplicationComponent : ComponentBase
    {
        [Parameter]
        public RestaurantApplication RestaurantApplication { get; set; }

        private string mainImagePath = string.Empty;

        protected override void OnParametersSet()
        {
            string imgName = RestaurantApplication.Images.First().ImagePath;
            mainImagePath = Path.Combine("/", "images", imgName);
        }
    }
}
