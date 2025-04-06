using System.ComponentModel.DataAnnotations;

namespace OnlineShop.Shared.Models
{
    public class RegisterRequest
    {
        [Required(AllowEmptyStrings = false, ErrorMessage = "Please enter your email!")]
        public string Email { get; set; }
        [Required(AllowEmptyStrings = false, ErrorMessage = "Please enter your password! It must contain at least one number and one uppercase and lowercase letter, and at least 8 or more characters")]
        public string Password { get; set; }

        public RegisterRequest()
        {

        }

        public RegisterRequest(string email, string password)
        {
            Email = email;
            Password = password;
        }
    }
}
