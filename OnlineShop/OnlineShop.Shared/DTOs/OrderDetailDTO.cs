using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineShop.Shared.DTOs
{
    public class OrderDetailDTO
    {
        public int Id { get; set; }
        public ProductDTO Product { get; set; }
        public double UnitPrice { get; set; }
        public int Quantity { get; set; }
    }
}
