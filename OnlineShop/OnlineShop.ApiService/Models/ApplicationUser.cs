using Microsoft.AspNetCore.Identity;

namespace OnlineShop.ApiService.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string UserId { get; set; }
    }
}
