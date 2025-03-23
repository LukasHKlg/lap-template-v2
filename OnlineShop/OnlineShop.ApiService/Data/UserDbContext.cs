using System.Reflection.Emit;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using OnlineShop.ApiService.Models;

namespace OnlineShop.ApiService.Data
{
    public class UserDbContext : IdentityDbContext<ApplicationUser>
    {
        public UserDbContext(DbContextOptions<UserDbContext> options) : base(options) { }

        public DbSet<CartItem> CartItems { get; set; }
        public DbSet<Cart> Carts { get; set; }
        public DbSet<SalesOrder> SalesOrders { get; set; }
        public DbSet<BillingAddress> BillingAddresses { get; set; }
        public DbSet<ShippingAddress> ShippingAddresses { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Customer> Customers { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Seed Products
            modelBuilder.Entity<Product>().HasData(
                new Product { Id = 1, Name = "Laptop", Description = "A powerful laptop", Manufacturer = "Dell", Category = "Electronics", Price = 1500.00, Stock = 20 },
                new Product { Id = 2, Name = "Smartphone", Description = "Latest model smartphone", Manufacturer = "Samsung", Category = "Electronics", Price = 800.00, Stock = 50 },
                new Product { Id = 3, Name = "Tablet", Description = "Portable tablet for everyday use", Manufacturer = "Apple", Category = "Electronics", Price = 600.00, Stock = 30 },
                new Product { Id = 4, Name = "Smartwatch", Description = "Wearable smart device", Manufacturer = "Fitbit", Category = "Wearables", Price = 250.00, Stock = 40 },
                new Product { Id = 5, Name = "Headphones", Description = "Noise-cancelling headphones", Manufacturer = "Bose", Category = "Audio", Price = 300.00, Stock = 15 },
                new Product { Id = 6, Name = "Camera", Description = "Digital SLR camera", Manufacturer = "Canon", Category = "Photography", Price = 1200.00, Stock = 10 },
                new Product { Id = 7, Name = "Printer", Description = "Wireless printer", Manufacturer = "HP", Category = "Office", Price = 200.00, Stock = 25 },
                new Product { Id = 8, Name = "Monitor", Description = "4K Ultra HD monitor", Manufacturer = "LG", Category = "Computers", Price = 400.00, Stock = 18 },
                new Product { Id = 9, Name = "Keyboard", Description = "Mechanical keyboard", Manufacturer = "Logitech", Category = "Computers", Price = 100.00, Stock = 35 },
                new Product { Id = 10, Name = "Mouse", Description = "Wireless mouse", Manufacturer = "Microsoft", Category = "Computers", Price = 50.00, Stock = 45 },
                new Product { Id = 11, Name = "Gaming Console", Description = "Next-gen gaming console", Manufacturer = "Sony", Category = "Gaming", Price = 500.00, Stock = 22 },
                new Product { Id = 12, Name = "VR Headset", Description = "Immersive virtual reality experience", Manufacturer = "Oculus", Category = "Gaming", Price = 350.00, Stock = 12 },
                new Product { Id = 13, Name = "Router", Description = "High-speed wireless router", Manufacturer = "Netgear", Category = "Networking", Price = 150.00, Stock = 28 },
                new Product { Id = 14, Name = "External Hard Drive", Description = "Portable storage device", Manufacturer = "Seagate", Category = "Computers", Price = 100.00, Stock = 40 },
                new Product { Id = 15, Name = "Smart TV", Description = "4K smart television", Manufacturer = "Samsung", Category = "Electronics", Price = 900.00, Stock = 8 },
                new Product { Id = 16, Name = "Bluetooth Speaker", Description = "Portable wireless speaker", Manufacturer = "JBL", Category = "Audio", Price = 120.00, Stock = 30 },
                new Product { Id = 17, Name = "Fitness Tracker", Description = "Wearable fitness tracker", Manufacturer = "Garmin", Category = "Wearables", Price = 80.00, Stock = 50 },
                new Product { Id = 18, Name = "Drone", Description = "Quadcopter drone with camera", Manufacturer = "DJI", Category = "Photography", Price = 750.00, Stock = 5 },
                new Product { Id = 19, Name = "E-Reader", Description = "Digital book reader", Manufacturer = "Amazon", Category = "Electronics", Price = 130.00, Stock = 20 },
                new Product { Id = 20, Name = "Smart Light", Description = "Wi-Fi enabled smart bulb", Manufacturer = "Philips", Category = "Home", Price = 30.00, Stock = 100 },
                new Product { Id = 21, Name = "Wireless Charger", Description = "Fast wireless charging pad", Manufacturer = "Anker", Category = "Accessories", Price = 40.00, Stock = 60 },
                new Product { Id = 22, Name = "Action Camera", Description = "Rugged action camera", Manufacturer = "GoPro", Category = "Photography", Price = 250.00, Stock = 14 },
                new Product { Id = 23, Name = "Laptop Stand", Description = "Ergonomic laptop stand", Manufacturer = "Rain Design", Category = "Accessories", Price = 70.00, Stock = 30 },
                new Product { Id = 24, Name = "Wireless Earbuds", Description = "True wireless stereo earbuds", Manufacturer = "Apple", Category = "Audio", Price = 200.00, Stock = 25 },
                new Product { Id = 25, Name = "Portable Charger", Description = "High-capacity power bank", Manufacturer = "RAVPower", Category = "Accessories", Price = 60.00, Stock = 35 }
            );

            // Seed Addresses
            modelBuilder.Entity<ShippingAddress>().HasData(
                new ShippingAddress
                {
                    Id = 1,
                    Street = "123 Main St",
                    HouseNum = "1A",
                    City = "Springfield",
                    State = "IL",
                    ZipCode = "62704",
                    Country = "USA"
                }
            );

            modelBuilder.Entity<BillingAddress>().HasData(
                new BillingAddress
                {
                    Id = 2,
                    Street = "456 Elm St",
                    HouseNum = "2B",
                    City = "Springfield",
                    State = "IL",
                    ZipCode = "62704",
                    Country = "USA"
                }
            );


            // Seed Cart
            modelBuilder.Entity<Cart>().HasData(
                new Cart
                {
                    Id = 1,
                    TotalPrice = 1500.00
                }
            );

            // Seed CartItem
            modelBuilder.Entity<CartItem>().HasData(
                new
                {
                    Id = 1,
                    Quantity = 1,
                    Price = 1500.00,
                    CartId = 1,
                    ProductId = 1
                }
            );

            // Seed Customer (Note: This is a customer without a login, to get a login user look at SeedUsersAndCustomers.cs file)
            modelBuilder.Entity<Customer>().HasData(
                new
                {
                    Id = 4,
                    FirstName = "Andre",
                    LastName = "Ludwig",
                    PhoneNumber = "555-1234",
                    ShippingAddressId = 1,
                    BillingAddressId = 2,
                    CartId = 1
                }
            );

            // Seed SalesOrder
            modelBuilder.Entity<SalesOrder>().HasData(
                new
                {
                    Id = 1,
                    // Similarly, use foreign key properties if available.
                    CartId = 1,
                    CustomerId = 4,
                    ShippingAddressId = 1,
                    BillingAddressId = 2
                }
            );
        }
    }
}
