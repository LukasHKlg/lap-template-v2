using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineShop.Shared.DTOs
{
    public class ProductDTO
    {
        public int Id { get; set; }
        [Required(AllowEmptyStrings = false, ErrorMessage = "Please enter a description!")]
        public string Description { get; set; }
        [Required(AllowEmptyStrings = false, ErrorMessage = "Please enter a Manufacturer!")]
        public string Manufacturer { get; set; }
        [Required(AllowEmptyStrings = false, ErrorMessage = "Please enter a Name for the product!")]
        public string Name { get; set; }
        [Required(AllowEmptyStrings = false, ErrorMessage = "Please enter a Category!")]
        public string Category { get; set; }
        [Required(ErrorMessage = "Please enter a Price!")]
        [Range(0, double.MaxValue, ErrorMessage = "Please enter valid double number!")]
        public double Price { get; set; }
        [Required(ErrorMessage = "Please enter how much is in stock!")]
        [Range(0, int.MaxValue, ErrorMessage = "Please enter valid integer number!")]
        public int Stock { get; set; }
    }
}
