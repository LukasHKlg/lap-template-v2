using System;
using System.Collections.Generic;
using System.Drawing.Printing;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OnlineShop.ApiService.Data;
using OnlineShop.ApiService.Models;
using OnlineShop.Shared.DTOs;
using OnlineShop.Shared.Models;

namespace OnlineShop.ApiService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomersController : ControllerBase
    {
        private readonly UserDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public CustomersController(UserDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: api/Customers
        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<PaginatedList<CustomerDTO>>> GetCustomers(int pageIndex, int pageSize)
        {
            if (pageIndex == 0) pageIndex = 1;

            var users = await _context.Customers.Include(o => o.ApplicationUser).Include(o => o.Cart).Include(o => o.BillingAddress).Include(o => o.ShippingAddress)
            .OrderBy(b => b.Id)
            .Skip((pageIndex - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

            if (users == null || users.Count == 0)
            {
                return NotFound();
            }

            var usersDto = users.Select(x =>
            new CustomerDTO()
            {
                Email = x.ApplicationUser?.Email,
                HasApplicationUser = x.ApplicationUser != null,
                isDisabled = x.ApplicationUser?.LockoutEnd != null ? true : false,
                Roles = x.ApplicationUser != null ? _userManager.GetRolesAsync(x.ApplicationUser).Result : [],
                Id = x.Id,
                FirstName = x.FirstName,
                LastName = x.LastName,
                PhoneNumber = x.PhoneNumber,
                Cart = new CartDTO() { Id = x.Cart.Id, TotalPrice = x.Cart.TotalPrice },
                BillingAddress = new BillingAddressDTO() { Id = x.BillingAddress.Id, City = x.BillingAddress.City, Country = x.BillingAddress.Country, State = x.BillingAddress.State, ZipCode = x.BillingAddress.ZipCode, HouseNum = x.BillingAddress.HouseNum, Street = x.BillingAddress.Street },
                ShippingAddress = new ShippingAddressDTO() { Id = x.ShippingAddress.Id, City = x.ShippingAddress.City, Country = x.ShippingAddress.Country, State = x.ShippingAddress.State, ZipCode = x.ShippingAddress.ZipCode, HouseNum = x.ShippingAddress.HouseNum, Street = x.ShippingAddress.Street }
            }).ToList();


            var count = await _context.Customers.CountAsync();
            var totalPages = (int)Math.Ceiling(count / (double)pageSize);

            return new PaginatedList<CustomerDTO>(usersDto, pageIndex, totalPages);
        }

        // GET: api/customers/search?searchValue
        [HttpGet]
        [Authorize(Roles = "Admin")]
        [Route("search")]
        public async Task<ActionResult<PaginatedList<CustomerDTO>>> GetCustomersForSearch(string searchValue, int pageIndex, int pageSize)
        {
            if (pageIndex == 0) pageIndex = 1;

            var customers = await _context.Customers.Include(o => o.ApplicationUser).Include(o => o.Cart).Include(o => o.BillingAddress).Include(o => o.ShippingAddress)
            .Where(x => x.FirstName.Contains(searchValue) ||
            x.LastName.Contains(searchValue) ||
            x.PhoneNumber.Contains(searchValue) ||
            x.ShippingAddress.State.Contains(searchValue) ||
            x.ShippingAddress.Country.Contains(searchValue) ||
            x.ShippingAddress.City.Contains(searchValue) ||
            x.BillingAddress.State.Contains(searchValue) ||
            x.BillingAddress.Country.Contains(searchValue) ||
            x.BillingAddress.City.Contains(searchValue))
            .OrderBy(b => b.Id)
            .Skip((pageIndex - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

            if (customers == null || customers.Count == 0) return NotFound();

            var customerDTOs = customers.Select(x =>
                new CustomerDTO()
                {
                    Email = x.ApplicationUser?.Email,
                    HasApplicationUser = x.ApplicationUser != null,
                    isDisabled = x.ApplicationUser?.LockoutEnd != null ? true : false,
                    Roles = x.ApplicationUser != null ? _userManager.GetRolesAsync(x.ApplicationUser).Result : [],
                    Id = x.Id,
                    FirstName = x.FirstName,
                    LastName = x.LastName,
                    PhoneNumber = x.PhoneNumber,
                    Cart = new CartDTO() { Id = x.Cart.Id, TotalPrice = x.Cart.TotalPrice },
                    BillingAddress = new BillingAddressDTO() { Id = x.BillingAddress.Id, City = x.BillingAddress.City, Country = x.BillingAddress.Country, State = x.BillingAddress.State, ZipCode = x.BillingAddress.ZipCode, HouseNum = x.BillingAddress.HouseNum, Street = x.BillingAddress.Street },
                    ShippingAddress = new ShippingAddressDTO() { Id = x.ShippingAddress.Id, City = x.ShippingAddress.City, Country = x.ShippingAddress.Country, State = x.ShippingAddress.State, ZipCode = x.ShippingAddress.ZipCode, HouseNum = x.ShippingAddress.HouseNum, Street = x.ShippingAddress.Street }
                }).ToList();

            var count = await _context.Customers.Include(o => o.ApplicationUser).Include(o => o.Cart).Include(o => o.BillingAddress).Include(o => o.ShippingAddress)
                        .Where(x => x.FirstName.Contains(searchValue) ||
                        x.LastName.Contains(searchValue) ||
                        x.PhoneNumber.Contains(searchValue) ||
                        x.ShippingAddress.State.Contains(searchValue) ||
                        x.ShippingAddress.Country.Contains(searchValue) ||
                        x.ShippingAddress.City.Contains(searchValue) ||
                        x.BillingAddress.State.Contains(searchValue) ||
                        x.BillingAddress.Country.Contains(searchValue) ||
                        x.BillingAddress.City.Contains(searchValue)).CountAsync();
            var totalPages = (int)Math.Ceiling(count / (double)pageSize);

            return new PaginatedList<CustomerDTO>(customerDTOs, pageIndex, totalPages);
        }

        // GET: api/Customers/5
        [HttpGet("{id}")]
        [Authorize]
        public async Task<ActionResult<CustomerDTO>> GetCustomer(int id)
        {
            //get authorized users token
            JwtSecurityToken jwtToken = new();
            if (HttpContext.Request.Headers.TryGetValue("Authorization", out var authHeader))
            {
                //"Bearer {token}"
                var token = authHeader.ToString().Split(" ").Last();

                // decode the JWT token
                var handler = new JwtSecurityTokenHandler();
                jwtToken = handler.ReadJwtToken(token);
            }
            else return NotFound();

            //get ApplicationUser user id
            var appUserId = jwtToken.Subject;

            var customer = await _context.Customers.Include(o => o.ApplicationUser).Include(o => o.Cart).Include(o => o.BillingAddress).Include(o => o.ShippingAddress)
                .Where(x => x.Id == id)
                .FirstOrDefaultAsync();

            if (customer == null)
            {
                return NotFound();
            }

            if (customer.ApplicationUser?.UserId != appUserId && jwtToken.Claims.Where(x => x.Type == ClaimTypes.Role && x.Value == Roles.Admin).FirstOrDefault()?.Value == null) return NotFound();



            var customerDto = new CustomerDTO()
            {
                Email = customer.ApplicationUser?.Email,
                HasApplicationUser = customer.ApplicationUser != null,
                isDisabled = customer.ApplicationUser?.LockoutEnd != null ? true : false,
                Roles = customer.ApplicationUser != null ? _userManager.GetRolesAsync(customer.ApplicationUser).Result : [],
                Id = customer.Id,
                FirstName = customer.FirstName,
                LastName = customer.LastName,
                PhoneNumber = customer.PhoneNumber,
                Cart = new CartDTO() { Id = customer.Cart.Id, TotalPrice = customer.Cart.TotalPrice },
                BillingAddress = new BillingAddressDTO() { Id = customer.BillingAddress.Id, City = customer.BillingAddress.City, Country = customer.BillingAddress.Country, State = customer.BillingAddress.State, ZipCode = customer.BillingAddress.ZipCode, HouseNum = customer.BillingAddress.HouseNum, Street = customer.BillingAddress.Street },
                ShippingAddress = new ShippingAddressDTO() { Id = customer.ShippingAddress.Id, City = customer.ShippingAddress.City, Country = customer.ShippingAddress.Country, State = customer.ShippingAddress.State, ZipCode = customer.ShippingAddress.ZipCode, HouseNum = customer.ShippingAddress.HouseNum, Street = customer.ShippingAddress.Street }
            };

            return customerDto;
        }

        // PUT: api/Customers/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        [Authorize]
        public async Task<IActionResult> PutCustomer(int id, CustomerDTO customerdto)
        {
            try
            {
                //TODO: check if id is same as in token or admin
                //get authorized users token
                JwtSecurityToken jwtToken = new();
                if (HttpContext.Request.Headers.TryGetValue("Authorization", out var authHeader))
                {
                    //"Bearer {token}"
                    var token = authHeader.ToString().Split(" ").Last();

                    // decode the JWT token
                    var handler = new JwtSecurityTokenHandler();
                    jwtToken = handler.ReadJwtToken(token);
                }
                else return NotFound();

                if (id != customerdto.Id && jwtToken.Claims.Where(x => x.Type == ClaimTypes.Role && x.Value == Roles.Admin).FirstOrDefault()?.Value == null)
                {
                    return BadRequest();
                }

                var customer = await _context.Customers.Include(o => o.ApplicationUser).Include(o => o.Cart).Include(o => o.BillingAddress).Include(o => o.ShippingAddress)
                    .Where(x => x.Id == id)
                    .FirstOrDefaultAsync();

                customer.LastName = customerdto.LastName;
                customer.FirstName = customerdto.FirstName;
                customer.PhoneNumber = customerdto.PhoneNumber;

                // Update ShippingAddress properties
                customer.ShippingAddress.City = customerdto.ShippingAddress.City;
                customer.ShippingAddress.Country = customerdto.ShippingAddress.Country;
                customer.ShippingAddress.HouseNum = customerdto.ShippingAddress.HouseNum;
                customer.ShippingAddress.ZipCode = customerdto.ShippingAddress.ZipCode;
                customer.ShippingAddress.State = customerdto.ShippingAddress.State;
                customer.ShippingAddress.Street = customerdto.ShippingAddress.Street;

                // Update BillingAddress properties
                customer.BillingAddress.City = customerdto.BillingAddress.City;
                customer.BillingAddress.Country = customerdto.BillingAddress.Country;
                customer.BillingAddress.HouseNum = customerdto.BillingAddress.HouseNum;
                customer.BillingAddress.ZipCode = customerdto.BillingAddress.ZipCode;
                customer.BillingAddress.State = customerdto.BillingAddress.State;
                customer.BillingAddress.Street = customerdto.BillingAddress.Street;

                // Update Cart properties
                customer.Cart.TotalPrice = customerdto.Cart.TotalPrice;

                _context.Entry(customer).State = EntityState.Modified;

                if(customer.ApplicationUser != null)
                {
                    foreach(var role in customerdto.Roles)
                    {
                        var alreadyInRole = await _userManager.IsInRoleAsync(customer.ApplicationUser, role);
                        if (!alreadyInRole) await _userManager.AddToRoleAsync(customer.ApplicationUser, role);
                        else await _userManager.RemoveFromRoleAsync(customer.ApplicationUser, role);
                    }
                }

                try
                {
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CustomerExists(id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }

                return NoContent();
            }
            catch (Exception)
            {

                throw;
            }
        }

        //// POST: api/Customers
        //// To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        //[HttpPost]
        //[Authorize(Roles = "Admin")]
        //public async Task<ActionResult<Customer>> PostCustomer(Customer customer)
        //{
        //    //TODO: check if id is same as in token or admin

        //    _context.Customers.Add(customer);
        //    await _context.SaveChangesAsync();

        //    return CreatedAtAction("GetCustomer", new { id = customer.Id }, customer);
        //}

        // DELETE: api/Customers/5
        [HttpDelete("{id}")]
        [Authorize]
        public async Task<IActionResult> DeleteCustomer(int id)
        {
            try
            {
                //get authorized users token
                JwtSecurityToken jwtToken = new();
                if (HttpContext.Request.Headers.TryGetValue("Authorization", out var authHeader))
                {
                    //"Bearer {token}"
                    var token = authHeader.ToString().Split(" ").Last();

                    // decode the JWT token
                    var handler = new JwtSecurityTokenHandler();
                    jwtToken = handler.ReadJwtToken(token);
                }
                else return NotFound();

                //get ApplicationUser user id
                var appUserId = jwtToken.Subject;

                var customer = await _context.Customers.Include(o => o.ApplicationUser).Include(o => o.Cart).Include(o => o.BillingAddress).Include(o => o.ShippingAddress)
                    .Where(x => x.Id == id)
                    .FirstOrDefaultAsync();

                if (customer.ApplicationUser?.UserId != appUserId && jwtToken.Claims.Where(x => x.Type == ClaimTypes.Role && x.Value == Roles.Admin).FirstOrDefault()?.Value == null) return NotFound();

                if (customer == null)
                {
                    return NotFound();
                }


                customer.LastName = "";
                customer.FirstName = "";
                customer.PhoneNumber = "";

                // Update ShippingAddress properties
                customer.ShippingAddress.City = "";
                customer.ShippingAddress.HouseNum = "";
                customer.ShippingAddress.ZipCode = "";
                customer.ShippingAddress.Street = "";

                // Update BillingAddress properties
                customer.BillingAddress.City = "";
                customer.BillingAddress.HouseNum = "";
                customer.BillingAddress.ZipCode = "";
                customer.BillingAddress.Street = "";

                await _userManager.SetLockoutEnabledAsync(customer.ApplicationUser, true);
                await _userManager.SetLockoutEndDateAsync(customer.ApplicationUser, DateTimeOffset.MaxValue);

                _context.Entry(customer).State = EntityState.Modified;
                await _context.SaveChangesAsync();

                return NoContent();
            }
            catch (Exception)
            {
                return BadRequest("There was an error while deleting the requested user.");
                throw;
            }
        }

        private bool CustomerExists(int id)
        {
            return _context.Customers.Any(e => e.Id == id);
        }
    }
}
