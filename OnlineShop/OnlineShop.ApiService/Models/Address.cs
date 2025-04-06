using System.ComponentModel.DataAnnotations;

namespace OnlineShop.ApiService.Models
{
    public class Address
    {
        [Key]
        public int Id { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string ZipCode { get; set; }
        public string Country { get; set; }
        public string Street { get; set; }
        public string HouseNum { get; set; }
    }
}
