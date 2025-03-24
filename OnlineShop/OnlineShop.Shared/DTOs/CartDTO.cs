using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineShop.Shared.DTOs
{
    public class CartDTO
    {
        public int Id { get; set; }
        public double TotalPrice { get; set; } = 0;
        [AllowNull]
        public List<CartItemDTO>? CartItems { get; set; }
    }
}
