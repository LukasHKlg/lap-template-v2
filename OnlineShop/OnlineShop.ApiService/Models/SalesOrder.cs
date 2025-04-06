using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using Microsoft.EntityFrameworkCore;

namespace OnlineShop.ApiService.Models
{
    public class SalesOrder
    {
        [Key]
        public int Id { get; set; }
        
        [DeleteBehavior(DeleteBehavior.NoAction)]
        public Customer Customer { get; set; }

        public DateTime OrderDate { get; set; }

        [AllowNull]
        public DateTime? ShippedDate { get; set; }

        //address
        public string ShipName { get; set; }
        public string ShipCountry { get; set; }
        public string ShipState { get; set; }
        public string ShipHouseNum { get; set; }
        public string ShipStreet { get; set; }
        public string ShipCity { get; set; }
        public string ShipZipCode { get; set; }

        public string BillingName { get; set; }
        public string BillingCountry { get; set; }
        public string BillingState { get; set; }
        public string BillingHouseNum { get; set; }
        public string BillingStreet { get; set; }
        public string BillingCity { get; set; }
        public string BillingZipCode { get; set; }
    }
}
