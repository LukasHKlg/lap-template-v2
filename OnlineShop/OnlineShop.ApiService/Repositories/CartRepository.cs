using Microsoft.EntityFrameworkCore;
using OnlineShop.ApiService.Data;
using OnlineShop.ApiService.Models;

namespace OnlineShop.ApiService.Repositories
{
    public class CartRepository
    {
        private readonly ILogger _logger;
        private UserDbContext _userDbContext;

        public CartRepository(ILogger<CartRepository> logger, UserDbContext userDbContext)
        {
            _logger = logger;
            _userDbContext = userDbContext;
        }

        public async Task<IEnumerable<CartItem>> GetCartItems(int cartId)
        {
            List<CartItem> cartItems = await _userDbContext.CartItems.Include(o => o.Product).Where(x => x.Cart.Id == cartId).ToListAsync();

            return cartItems;
        }
    }
}
