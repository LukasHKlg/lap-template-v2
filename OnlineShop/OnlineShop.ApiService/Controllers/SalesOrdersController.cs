using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OnlineShop.ApiService.Data;
using OnlineShop.ApiService.Models;
using OnlineShop.ApiService.Repositories;
using OnlineShop.ApiService.Services;
using OnlineShop.Shared.Models;
using QuestPDF.Fluent;

namespace OnlineShop.ApiService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class SalesOrdersController : ControllerBase
    {
        private readonly UserDbContext _context;
        private CartRepository CartRepository;

        public SalesOrdersController(UserDbContext context, CartRepository cartRepository)
        {
            _context = context;
            CartRepository = cartRepository;
        }

        // GET: api/SalesOrders
        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<IEnumerable<SalesOrder>>> GetSalesOrders()
        {
            return await _context.SalesOrders.ToListAsync();
        }

        // GET: api/SalesOrders/5
        [HttpGet("{id}")]
        public async Task<ActionResult<SalesOrder>> GetSalesOrder(int id)
        {
            var salesOrder = await _context.SalesOrders.FindAsync(id);

            if (salesOrder == null)
            {
                return NotFound();
            }

            return salesOrder;
        }

        //// PUT: api/SalesOrders/5
        //// To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        //[HttpPut("{id}")]
        //public async Task<IActionResult> PutSalesOrder(int id, SalesOrder salesOrder)
        //{
        //    if (id != salesOrder.Id)
        //    {
        //        return BadRequest();
        //    }

        //    _context.Entry(salesOrder).State = EntityState.Modified;

        //    try
        //    {
        //        await _context.SaveChangesAsync();
        //    }
        //    catch (DbUpdateConcurrencyException)
        //    {
        //        if (!SalesOrderExists(id))
        //        {
        //            return NotFound();
        //        }
        //        else
        //        {
        //            throw;
        //        }
        //    }

        //    return NoContent();
        //}

        // POST: api/SalesOrders
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<SalesOrder>> PostSalesOrder(SalesOrder salesOrder)
        {
            var addedOrder = _context.SalesOrders.Add(salesOrder);
            await _context.SaveChangesAsync();

            return addedOrder.Entity;
        }

        //// DELETE: api/SalesOrders/5
        //[HttpDelete("{id}")]
        //public async Task<IActionResult> DeleteSalesOrder(int id)
        //{
        //    var salesOrder = await _context.SalesOrders.FindAsync(id);
        //    if (salesOrder == null)
        //    {
        //        return NotFound();
        //    }

        //    _context.SalesOrders.Remove(salesOrder);
        //    await _context.SaveChangesAsync();

        //    return NoContent();
        //}

        private bool SalesOrderExists(int id)
        {
            return _context.SalesOrders.Any(e => e.Id == id);
        }


        [HttpGet("getinvoice/{id}")]
        public async Task<IActionResult> GetInvoice(int id)
        {
            try
            {
                var order = await _context.SalesOrders.Include(x => x.BillingAddress).Include(x => x.ShippingAddress).Include(x => x.Cart)
                        .Include(x => x.Customer).FirstOrDefaultAsync(x => x.Id == id);
                if (order == null) { return NotFound(); }

                var cartItems = await CartRepository.GetCartItems(order.Cart.Id);

                var invoice = new GeneratePDFInvoice(order, cartItems.ToList());

                var stream = new MemoryStream();
                invoice.GeneratePdf(stream);
                stream.Position = 0;

                return File(stream, "application/pdf", $"invoice_{id}.pdf");
            }
            catch (Exception)
            {
                return BadRequest("Could not create the invoice.");
                throw;
            }
        }
    }
}
