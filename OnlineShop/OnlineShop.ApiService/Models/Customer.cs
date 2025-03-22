using System.ComponentModel.DataAnnotations;

namespace OnlineShop.ApiService.Models
{
    public class Customer
    {
        [Key]
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string PhoneNumber { get; set; }

        public ShippingAddress ShippingAddress { get; set; }
        public BillingAddress BillingAddress { get; set; }
        public ApplicationUser ApplicationUser { get; set; }
        public Cart Cart { get; set; }
    }
}
