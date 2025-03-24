using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace OnlineShop.ApiService.Models
{
    public class CartItem
    {
        [Key]
        public int Id { get; set; }
        public int Quantity { get; set; }
        public double Price { get; set; }
        public Cart Cart { get; set; }
        //[DeleteBehavior(DeleteBehavior.Cascade)] TODO: check what happens when product is deleted
        public Product Product { get; set; }
    }
}
