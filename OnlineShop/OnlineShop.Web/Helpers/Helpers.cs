using System.Net.Http.Headers;
using System.Net.Http;

namespace OnlineShop.Web.Helpers
{
    public class Helpers
    {
        public static void GetUserAuthToken(HttpContext httpContext, ref HttpClient httpClient)
        {
            // Retrieve the JWT token from the current user's claims.
            var token = httpContext?.User.FindFirst("access_token")?.Value;
            if (!string.IsNullOrEmpty(token))
            {
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            }
            else
            {
                throw new Exception("No access token found.");
            }
        }

        public static string GetPictureUrl(string picturePath, AppsettingsConfigService configHelper)
        {
            picturePath = picturePath.Contains('.') ? picturePath : picturePath + ".jpg";

            return configHelper.APIPathProductPicturesPath + picturePath;
        }
    }
}
