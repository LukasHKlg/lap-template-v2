using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace OnlineShop.ApiService.Models
{
    public class SalesOrder
    {
        [Key]
        public int Id { get; set; }
        public DateTime OrderDate { get; set; }
        public Cart Cart { get; set; }
        [DeleteBehavior(DeleteBehavior.NoAction)]
        public Customer Customer { get; set; }
        public ShippingAddress ShippingAddress { get; set; }
        public BillingAddress BillingAddress { get; set; }
    }
}
