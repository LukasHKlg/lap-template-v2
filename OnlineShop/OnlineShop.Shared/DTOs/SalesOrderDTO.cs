using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineShop.Shared.DTOs
{
    public class SalesOrderDTO
    {
        public int Id { get; set; }
        public CartDTO Cart { get; set; }
        public CustomerDTO Customer { get; set; }
        public ShippingAddressDTO ShippingAddress { get; set; }
        public BillingAddressDTO BillingAddress { get; set; }
    }
}
