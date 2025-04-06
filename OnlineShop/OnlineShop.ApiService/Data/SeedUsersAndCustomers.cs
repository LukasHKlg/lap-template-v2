using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using OnlineShop.ApiService.Models;
using OnlineShop.Shared.Models;

namespace OnlineShop.ApiService.Data
{
    public class SeedUsersAndCustomers
    {
        public static async Task SeedUsersAndCustomersAsync(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager, UserDbContext context)
        {
            //if this failes you possibly forgot to load migrations into database
            #region Roles
            foreach (var role in Roles.SelectableRoles)
            {
                if (!await roleManager.RoleExistsAsync(role))
                {
                    await roleManager.CreateAsync(new IdentityRole(role));
                }
            }
            #endregion Roles

            #region AdminUser
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
            if (!await userManager.IsInRoleAsync(adminuser, Roles.Admin))
            {
                await userManager.AddToRoleAsync(adminuser, Roles.Admin);
            }

            // Now update or seed the Customer record with this ApplicationUser's id.
            if (!context.Customers.Any(x => x.ApplicationUser.Id == adminuser.Id))
            {
                var customer = new Customer
                {
                    FirstName = "John",
                    LastName = "Doe",
                    PhoneNumber = "555-1234",
                    ApplicationUser = adminuser, // Now you have the id from the created user.
                    BillingAddress =   new() {
                        Street = "Lausing",
                        HouseNum = "15",
                        City = "Wolfsberg",
                        State = "Kärnten",
                        ZipCode = "9411",
                        Country = "Österreich"
                    },
                    Cart = new(),
                    ShippingAddress = new()
                    {
                        Street = "Lausing",
                        HouseNum = "15",
                        City = "Wolfsberg",
                        State = "Kärnten",
                        ZipCode = "9411",
                        Country = "Österreich"
                    }// Set other required properties/foreign keys
                };
                context.Customers.Add(customer);
                await context.SaveChangesAsync();
            }
            #endregion AdminUser

            #region CustomerUser
            var customeruser1 = await userManager.FindByEmailAsync("customer1@example.com");
            if (customeruser1 == null)
            {
                customeruser1 = new ApplicationUser
                {
                    Id = "00000000-0000-0000-0000-000000000002", // You can assign a fixed id here
                    UserId = "00000000-0000-0000-0000-000000000002",
                    UserName = "customer1@example.com",
                    Email = "customer1@example.com"
                };

                var result = await userManager.CreateAsync(customeruser1, "TestPassword123!");
                if (!result.Succeeded)
                {
                    throw new Exception("Failed to create user: " + string.Join(", ", result.Errors.Select(e => e.Description)));
                }
            }
            if (!await userManager.IsInRoleAsync(customeruser1, Roles.User))
            {
                await userManager.AddToRoleAsync(customeruser1, Roles.User);
            }

            // Now update or seed the Customer record with this ApplicationUser's id.
            if (!context.Customers.Any(x => x.ApplicationUser.Id == customeruser1.Id))
            {
                var customer = new Customer
                {
                    FirstName = "Werner",
                    LastName = "Rose",
                    PhoneNumber = "93243847362",
                    ApplicationUser = customeruser1, // Now you have the id from the created user.
                    BillingAddress = new() {
                        Street = "Lausing",
                        HouseNum = "5",
                        City = "St. Michale",
                        State = "Kärnten",
                        ZipCode = "9411",
                        Country = "Österreich"
                    },
                    Cart = new(),
                    ShippingAddress = new()
                    {
                        Street = "Lausing",
                        HouseNum = "5",
                        City = "St. Michale",
                        State = "Kärnten",
                        ZipCode = "9411",
                        Country = "Österreich"
                    }// Set other required properties/foreign keys
                };
                context.Customers.Add(customer);
                await context.SaveChangesAsync();
            }

            var customeruser2 = await userManager.FindByEmailAsync("customer2@example.com");
            if (customeruser2 == null)
            {
                customeruser2 = new ApplicationUser
                {
                    Id = "00000000-0000-0000-0000-000000000003", // You can assign a fixed id here
                    UserId = "00000000-0000-0000-0000-000000000003",
                    UserName = "customer2@example.com",
                    Email = "customer2@example.com"
                };

                var result = await userManager.CreateAsync(customeruser2, "TestPassword123!");
                if (!result.Succeeded)
                {
                    throw new Exception("Failed to create user: " + string.Join(", ", result.Errors.Select(e => e.Description)));
                }
            }
            if (!await userManager.IsInRoleAsync(customeruser2, Roles.User))
            {
                await userManager.AddToRoleAsync(customeruser2, Roles.User);
            }

            // Now update or seed the Customer record with this ApplicationUser's id.
            if (!context.Customers.Any(x => x.ApplicationUser.Id == customeruser2.Id))
            {
                var customer = new Customer
                {
                    FirstName = "Lukas",
                    LastName = "Scholz",
                    PhoneNumber = "123478903",
                    ApplicationUser = customeruser2, // Now you have the id from the created user.
                    BillingAddress = new() {
                        Street = "Arnulfplatz",
                        HouseNum = "1",
                        City = "Klagenfurt",
                        State = "Kärnten",
                        ZipCode = "9010",
                        Country = "Kärnten"
                    },
                    Cart = new(),
                    ShippingAddress = new()
                    {
                        Street = "Arnulfplatz",
                        HouseNum = "1",
                        City = "Klagenfurt",
                        State = "Kärnten",
                        ZipCode = "9010",
                        Country = "Kärnten"
                    }// Set other required properties/foreign keys
                };
                context.Customers.Add(customer);
                await context.SaveChangesAsync();
            }
            #endregion CustomerUser
        }
    }
}
