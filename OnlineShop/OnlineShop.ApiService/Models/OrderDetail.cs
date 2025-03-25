using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OnlineShop.ApiService.Models
{
    public class OrderDetail
    {
        [Key]
        public int Id { get; set; }
        [ForeignKey("OrderId")]
        public SalesOrder SalesOrder { get; set; }
        [ForeignKey("ProductId")]
        public Product Product { get; set; }
        public double UnitPrice { get; set; }
        public int Quantity { get; set; }
    }
}
