using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OnlineShop.Shared.Models;

namespace OnlineShop.Shared.DTOs
{
    public class CustomerDTO
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }

        public ShippingAddressDTO ShippingAddress { get; set; }
        public BillingAddressDTO BillingAddress { get; set; }

        //public ApplicationUser ApplicationUser { get; set; }
        public bool HasApplicationUser { get; set; }
        public bool isDisabled { get; set; }
        [AllowNull]
        public IEnumerable<string>? Roles { get; set; }
        [AllowNull]
        public CartDTO? Cart { get; set; }
    }
}
