using Microsoft.AspNetCore.Identity;

namespace OnlineShop.ApiService.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string UserId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string CustomerPhone { get; set; }
    }
}
