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
        public CustomerDTO Customer { get; set; }


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

        [AllowNull]
        public List<OrderDetailDTO>? OrderItems { get; set; }
    }
}
