using System.ComponentModel.DataAnnotations;

namespace OnlineShop.ApiService.Models
{
    public class Cart
    {
        [Key]
        public int Id { get; set; }
        public double TotalPrice { get; set; } = 0;
    }
}
