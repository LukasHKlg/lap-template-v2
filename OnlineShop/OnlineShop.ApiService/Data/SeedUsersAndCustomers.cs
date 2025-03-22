using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using OnlineShop.ApiService.Models;

namespace OnlineShop.ApiService.Data
{
    public class SeedUsersAndCustomers
    {
        public static async Task SeedUsersAndCustomersAsync(UserManager<ApplicationUser> userManager, UserDbContext context)
        {
            var adminuser = await userManager.FindByEmailAsync("admin@example.com");
            if (adminuser == null)
            {
                adminuser = new ApplicationUser
                {
                    Id = "00000000-0000-0000-0000-000000000001", // You can assign a fixed id here
                    UserId = "00000000-0000-0000-0000-000000000001",
                    UserName = "admin@example.com",
                    Email = "admin@example.com"
                };

                var result = await userManager.CreateAsync(adminuser, "TestPassword123!");
                if (!result.Succeeded)
                {
                    throw new Exception("Failed to create user: " + string.Join(", ", result.Errors.Select(e => e.Description)));
                }
            }

            // Now update or seed the Customer record with this ApplicationUser's id.
            if (!context.Customers.Any())
            {
                var customer = new Customer
                {
                    Id = 1,
                    FirstName = "John",
                    LastName = "Doe",
                    PhoneNumber = "555-1234",
                    ApplicationUser = adminuser, // Now you have the id from the created user.
                                                 // Set other required properties/foreign keys
                };
                context.Customers.Add(customer);
                await context.SaveChangesAsync();
            }
        }
    }
}
