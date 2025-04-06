using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OnlineShop.ApiService.Data;
using OnlineShop.ApiService.Models;
using OnlineShop.ApiService.Repositories;
using OnlineShop.Shared.DTOs;

namespace OnlineShop.ApiService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CartsController : ControllerBase
    {
        private readonly UserDbContext _context;
        private readonly CartRepository _cartRepository;
        private readonly ILogger<CartsController> _logger;

        public CartsController(UserDbContext context, CartRepository cartRepository, ILogger<CartsController> logger)
        {
            _context = context;
            _cartRepository = cartRepository;
            _logger = logger;
        }

        // GET: api/Carts
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Cart>>> GetCarts()
        {
            return await _context.Carts.ToListAsync();
        }

        // GET: api/carts/getcartforuser
        [Authorize]
        [HttpGet]
        [Route("getcartforuser")]
        public async Task<ActionResult<CartDTO>> GetCartForUser()
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
                } else NotFound();

                //get ApplicationUser user id
                var id = jwtToken.Subject;

                //get Customer from db for personal cart
                var user = await _context.Customers.Include(o => o.Cart).Where(x => x.ApplicationUser.UserId == id).FirstOrDefaultAsync();
                var cart = user?.Cart;

                if (cart == null)
                {
                    return NotFound();
                }

                //get all CartItems
                var cartItems = await _cartRepository.GetCartItems(cart.Id);

                var cartItemsDto = cartItems.Select(x =>
                new CartItemDTO
                {
                    Id = x.Id,
                    Price = x.Price,
                    Product = new ProductDTO() { Price = x.Product.Price, Category = x.Product.Category, Description = x.Product.Description, Id = x.Product.Id, Manufacturer = x.Product.Manufacturer, Name = x.Product.Name, Stock = x.Product.Stock },
                    Cart = new CartDTO() { Id = x.Cart.Id, TotalPrice = x.Cart.TotalPrice },
                    Quantity = x.Quantity
                });

                var cartDto = new CartDTO()
                {
                    Id = cart.Id,
                    TotalPrice = cart.TotalPrice,
                    CartItems = cartItemsDto.ToList()
                };

                return cartDto;
            }
            catch (Exception ex)
            {
                _logger.LogError("Error while getting the Cart with CartItems. Error: " + ex.ToString());
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        // PUT: api/Carts/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCart(int id, Cart cart)
        {
            if (id != cart.Id)
            {
                return BadRequest();
            }

            _context.Entry(cart).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CartExists(id))
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

        // POST: api/Carts
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Cart>> PostCart(Cart cart)
        {
            _context.Carts.Add(cart);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetCart", new { id = cart.Id }, cart);
        }

        // DELETE: api/Carts/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCart(int id)
        {
            var cart = await _context.Carts.FindAsync(id);
            if (cart == null)
            {
                return NotFound();
            }

            _context.Carts.Remove(cart);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool CartExists(int id)
        {
            return _context.Carts.Any(e => e.Id == id);
        }
    }
}
