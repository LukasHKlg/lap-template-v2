using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineShop.Shared.DTOs
{
    public class SalesOrderDTO
    {
        public int Id { get; set; }
        [AllowNull]
        public CustomerDTO? Customer { get; set; }

        [AllowNull]
        public DateTime? OrderDate { get; set; }

        [AllowNull]
        public DateTime? ShippedDate { get; set; }

        //address
        [AllowNull]
        public string? ShipName { get; set; }
        [AllowNull]
        public string? ShipCountry { get; set; }
        [AllowNull]
        public string? ShipState { get; set; }
        [AllowNull]
        public string? ShipHouseNum { get; set; }
        [AllowNull]
        public string? ShipStreet { get; set; }
        [AllowNull]
        public string? ShipCity { get; set; }
        [AllowNull]
        public string? ShipZipCode { get; set; }
        [AllowNull]
        public string? BillingName { get; set; }
        [AllowNull]
        public string? BillingCountry { get; set; }
        [AllowNull]
        public string? BillingState { get; set; }
        [AllowNull]
        public string? BillingHouseNum { get; set; }
        [AllowNull]
        public string? BillingStreet { get; set; }
        [AllowNull]
        public string? BillingCity { get; set; }
        [AllowNull]
        public string? BillingZipCode { get; set; }

        [AllowNull]
        public List<OrderDetailDTO>? OrderItems { get; set; }
    }
}
