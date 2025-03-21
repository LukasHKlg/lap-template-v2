namespace OnlineShop.ApiService.Models
{
    public class RegisterRequest
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Username { get; set; }
        public string CustomerPhone { get; set; }

        public RegisterRequest()
        {

        }

        public RegisterRequest(string email, string password, string firstName, string lastName, string username, string customerPhone)
        {
            Email = email;
            Password = password;
            FirstName = firstName;
            LastName = lastName;
            Username = username;
            CustomerPhone = customerPhone;
        }
    }
}
