using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineShop.Shared.DTOs
{
    public class ProductDTO
    {
        public int Id { get; set; }
        public string Description { get; set; }
        public string Manufacturer { get; set; }
        public string Name { get; set; }
        public string Category { get; set; }
        public double Price { get; set; }
        public int Stock { get; set; }
    }
}
