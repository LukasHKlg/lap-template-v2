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
                new Product
                {
                    Id = 1,
                    Name = "Laptop",
                    Description = "A powerful laptop",
                    Manufacturer = "Dell",
                    Category = "Electronics",
                    Price = 1500.00,
                    Stock = 20
                },
                new Product
                {
                    Id = 2,
                    Name = "Smartphone",
                    Description = "Latest model smartphone",
                    Manufacturer = "Samsung",
                    Category = "Electronics",
                    Price = 800.00,
                    Stock = 50
                }
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
