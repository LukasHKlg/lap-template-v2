using System.ComponentModel.DataAnnotations;

namespace OnlineShop.Shared.Models
{
    public class LoginRequest
    {
        [Required(AllowEmptyStrings = false, ErrorMessage = "Please enter your email!")]
        public string Email { get; set; }
        [Required(AllowEmptyStrings = false, ErrorMessage = "Please enter your password!")]
        public string Password { get; set; }

        public LoginRequest()
        {
            
        }

        public LoginRequest(string email, string password)
        {
            Email = email;
            Password = password;
        }
    }
}
