using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineShop.Shared.DTOs
{
    public class CustomerDTO
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string PhoneNumber { get; set; }

        public ShippingAddressDTO ShippingAddress { get; set; }
        public BillingAddressDTO BillingAddress { get; set; }
        //public ApplicationUser ApplicationUser { get; set; }
        public CartDTO Cart { get; set; }
    }
}
