using Microsoft.AspNetCore.Components.Forms;

namespace Resaplace
{
    public static class Utils
    {
        public static string GetFileExtension(IBrowserFile image)
        {
            return image.ContentType switch
            {
                "image/png" => ".png",
                "image/jpeg" => ".jpeg",
                _ => ".tmp",
            };
        }
    }
}
