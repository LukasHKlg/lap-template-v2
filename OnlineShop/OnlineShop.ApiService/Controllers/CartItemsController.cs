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
using OnlineShop.Shared.DTOs;

namespace OnlineShop.ApiService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CartItemsController : ControllerBase
    {
        private readonly UserDbContext _context;

        public CartItemsController(UserDbContext context)
        {
            _context = context;
        }

        //// GET: api/CartItems
        //[HttpGet]
        //public async Task<ActionResult<IEnumerable<CartItem>>> GetCartItems()
        //{
        //    return await _context.CartItems.ToListAsync();
        //}

        //// GET: api/CartItems/5
        //[HttpGet("{id}")]
        //public async Task<ActionResult<CartItem>> GetCartItem(int id)
        //{
        //    var cartItem = await _context.CartItems.FindAsync(id);

        //    if (cartItem == null)
        //    {
        //        return NotFound();
        //    }

        //    return cartItem;
        //}

        // PUT: api/CartItems/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [Authorize]
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCartItem(int id, CartItem cartItem)
        {
            if (id != cartItem.Id)
            {
                return BadRequest();
            }
                
            _context.Entry(cartItem).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CartItemExists(id))
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

        // POST: api/CartItems
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [Authorize]
        [HttpGet("{id}")]
        public async Task<ActionResult<CartItemDTO>> PostCartItem(int id)
        {
            //get users token
            JwtSecurityToken jwtToken = new();
            if (HttpContext.Request.Headers.TryGetValue("Authorization", out var authHeader))
            {
                //"Bearer {token}"
                var token = authHeader.ToString().Split(" ").Last();

                // decode the JWT token
                var handler = new JwtSecurityTokenHandler();
                jwtToken = handler.ReadJwtToken(token);
            }
            else NotFound();

            //get ApplicationUser user id
            var userid = jwtToken.Subject;

            //get users cart
            var user = await _context.Customers.Include(o => o.Cart).Where(x => x.ApplicationUser.UserId == userid).FirstOrDefaultAsync();
            var cart = user?.Cart;

            //if no cart create cart for customer
            if (cart == null)
            {
                var newCart = new Cart();
                _context.Carts.Add(newCart);
                _context.SaveChanges();

                //get the cart again
                user = await _context.Customers.Include(o => o.Cart).Where(x => x.ApplicationUser.UserId == userid).FirstOrDefaultAsync();
                cart = user?.Cart;
            }

            //get product for prod id
            var newProduct = await _context.Products.FindAsync(id);

            //try get already existing cartitem
            var cartItem = await _context.CartItems.FirstOrDefaultAsync(x => x.Product.Id == newProduct.Id);
            if(cartItem == null)
            {
                //create new CartItem
                cartItem = new CartItem()
                {
                    Product = newProduct,
                    Cart = cart,
                    Price = newProduct.Price,
                    Quantity = 1
                };
                //add new CartItem
                _context.CartItems.Add(cartItem);
            }
            else
            {
                cartItem.Quantity += 1;
                cartItem.Price += cartItem.Product.Price;

                //update existing cartitem
                _context.Entry(cartItem).State = EntityState.Modified;
            }

                
            await _context.SaveChangesAsync();

            return new CartItemDTO() { };

            //return CreatedAtAction("GetCartItem", new { id = cartItem.Id }, cartItem);
        }

        // DELETE: api/CartItems/5
        [Authorize]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCartItem(int id)
        {
            var cartItem = await _context.CartItems.FindAsync(id);
            if (cartItem == null)
            {
                return NotFound();
            }

            _context.CartItems.Remove(cartItem);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool CartItemExists(int id)
        {
            return _context.CartItems.Any(e => e.Id == id);
        }
    }
}
