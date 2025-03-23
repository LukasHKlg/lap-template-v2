using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
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
    public class ProductsController : ControllerBase
    {
        private readonly UserDbContext _context;

        public ProductsController(UserDbContext context)
        {
            _context = context;
        }

        // GET: api/Products
        [HttpGet]
        public async Task<ActionResult<PaginatedList<ProductDTO>>> GetProducts(int pageIndex, int pageSize)
        {
            //return await _context.Products.ToListAsync();

            var products = await _context.Products
            .OrderBy(b => b.Id)
            .Skip((pageIndex - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

            if (products == null || products.Count == 0) return NotFound();

            var productDTOs = products.Select(x =>
                new ProductDTO
                {
                    Id = x.Id,
                    Name = x.Name,
                    Category = x.Category,
                    Description = x.Description,
                    Manufacturer = x.Manufacturer,
                    Price = x.Price,
                    Stock = x.Stock
                }).ToList();

            var count = await _context.Products.CountAsync();
            var totalPages = (int)Math.Ceiling(count / (double)pageSize);

            return new PaginatedList<ProductDTO>(productDTOs, pageIndex, totalPages);
        }

        // GET: api/Products?searchValue
        [HttpGet]
        [Route("search")]
        public async Task<ActionResult<PaginatedList<ProductDTO>>> GetProductsForSearch(string searchValue, int pageIndex, int pageSize)
        {
            //return await _context.Products.ToListAsync();

            if (pageIndex == 0) pageIndex = 1;

            var products = await _context.Products
            .Where(x => x.Name.Contains(searchValue) ||
            x.Manufacturer.Contains(searchValue) ||
            x.Category.Contains(searchValue) ||
            x.Description.Contains(searchValue))
            .OrderBy(b => b.Id)
            .Skip((pageIndex - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

            if (products == null || products.Count == 0) return NotFound();

            var productDTOs = products.Select(x =>
                new ProductDTO
                {
                    Id = x.Id,
                    Name = x.Name,
                    Category = x.Category,
                    Description = x.Description,
                    Manufacturer = x.Manufacturer,
                    Price = x.Price,
                    Stock = x.Stock
                }).ToList();

            var count = await _context.Products.Where(x => x.Name.Contains(searchValue) ||
                                                        x.Manufacturer.Contains(searchValue) ||
                                                        x.Category.Contains(searchValue) ||
                                                        x.Description.Contains(searchValue)
                                                        ).CountAsync();
            var totalPages = (int)Math.Ceiling(count / (double)pageSize);

            return new PaginatedList<ProductDTO>(productDTOs, pageIndex, totalPages);
        }

        // GET: api/Products/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Product>> GetProduct(int id)
        {
            var product = await _context.Products.FindAsync(id);

            if (product == null)
            {
                return NotFound();
            }

            return product;
        }

        // PUT: api/Products/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutProduct(int id, Product product)
        {
            if (id != product.Id)
            {
                return BadRequest();
            }

            _context.Entry(product).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProductExists(id))
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

        // POST: api/Products
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Product>> PostProduct(Product product)
        {
            _context.Products.Add(product);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetProduct", new { id = product.Id }, product);
        }

        // DELETE: api/Products/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            var product = await _context.Products.FindAsync(id);
            if (product == null)
            {
                return NotFound();
            }

            _context.Products.Remove(product);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ProductExists(int id)
        {
            return _context.Products.Any(e => e.Id == id);
        }
    }
}
