using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineShop.Shared.DTOs
{
    public class CartItemDTO
    {
        public int Id { get; set; }
        public int Quantity { get; set; }
        public double Price { get; set; }
        public CartDTO? Cart { get; set; }
        public ProductDTO Product { get; set; }
    }
}
