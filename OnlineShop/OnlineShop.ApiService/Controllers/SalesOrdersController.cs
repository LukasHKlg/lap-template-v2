﻿using System;
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
using OnlineShop.ApiService.Services;
using OnlineShop.Shared.DTOs;
using OnlineShop.Shared.Models;
using QuestPDF.Fluent;
using QuestPDF.Helpers;

namespace OnlineShop.ApiService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class SalesOrdersController : ControllerBase
    {
        private readonly UserDbContext _context;
        private CartRepository CartRepository;
        private readonly ILogger<SalesOrdersController> _logger;

        public SalesOrdersController(UserDbContext context, CartRepository cartRepository, ILogger<SalesOrdersController> logger)
        {
            _context = context;
            CartRepository = cartRepository;
            _logger = logger;
        }

        // GET: api/SalesOrders
        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<PaginatedList<SalesOrderDTO>>> GetSalesOrders(int pageIndex, int pageSize)
        {
            var salesOrders = await _context.SalesOrders
                .OrderBy(b => b.Id)
                .Skip((pageIndex - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            if (salesOrders == null || salesOrders.Count == 0) return NotFound();

            var salesOrdersDto = salesOrders.Select(x => new SalesOrderDTO()
            {
                OrderItems = _context.OrderDetails.Where(y => y.SalesOrder.Id == x.Id).Select(z => new OrderDetailDTO() { Id = z.Id, Product = new ProductDTO() { Id = z.Product.Id }, Quantity = z.Quantity, UnitPrice = z.UnitPrice }).ToList(),
                Id = x.Id,
                ShippedDate = x.ShippedDate,
                OrderDate = x.OrderDate,
                ShipName = x.ShipName,
            }).ToList();

            var count = await _context.SalesOrders.CountAsync();
            var totalPages = (int)Math.Ceiling(count / (double)pageSize);

            return new PaginatedList<SalesOrderDTO>(salesOrdersDto, pageIndex, totalPages);
        }

        // GET: api/SalesOrders/ordersuser
        [HttpGet]
        [Route("ordersuser")]
        public async Task<ActionResult<IEnumerable<SalesOrderDTO>>> GetSalesOrdersForCustomer()
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

            var customer = await _context.Customers.FirstOrDefaultAsync(x => x.ApplicationUser.UserId == userid);

            if (customer == null) return NotFound();

            var ordersForUser = _context.SalesOrders.Where(x => x.Customer.Id == customer.Id).ToList();

            var ordersForUserDto = ordersForUser.Select(x => new SalesOrderDTO()
            {
                OrderItems = _context.OrderDetails.Where(y => y.SalesOrder.Id == x.Id).Select(z => new OrderDetailDTO() {Id = z.Id, Product = new ProductDTO() { Id = z.Product.Id }, Quantity = z.Quantity, UnitPrice = z.UnitPrice }).ToList(),
                Id = x.Id,
                ShippedDate = x.ShippedDate,
                OrderDate = x.OrderDate,
                ShipName = x.ShipName,
            }).ToList();

            return ordersForUserDto;
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

        // PUT: api/SalesOrders/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> PutSalesOrder(int id, SalesOrderDTO salesOrder)
        {
            if (id != salesOrder.Id)
            {
                return BadRequest();
            }

            var order = await _context.SalesOrders.FindAsync(id);

            if (order == null) return NotFound();

            order.ShippedDate = salesOrder.ShippedDate;

            _context.Entry(order).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!SalesOrderExists(id))
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

        // POST: api/SalesOrders
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<SalesOrderDTO>> PostSalesOrder(SalesOrderDTO salesOrderdto)
        {
            try
            {


                var user = await _context.Customers.Include(o => o.Cart).FirstOrDefaultAsync(x => x.Id == salesOrderdto.Customer.Id);
                //foreach product in cart add to order orderdtails

                var salesOrder = new SalesOrder()
                {
                    BillingCity = salesOrderdto.BillingCity,
                    BillingCountry = salesOrderdto.BillingCountry,
                    BillingHouseNum = salesOrderdto.BillingHouseNum,
                    BillingName = salesOrderdto.BillingName,
                    BillingState = salesOrderdto.BillingState,
                    BillingStreet = salesOrderdto.BillingStreet,
                    BillingZipCode = salesOrderdto.BillingZipCode,
                    Customer = user,
                    ShipCity = salesOrderdto.ShipCity,
                    OrderDate = (DateTime)salesOrderdto.OrderDate,
                    ShipCountry = salesOrderdto.ShipCountry,
                    ShipHouseNum = salesOrderdto.ShipHouseNum,
                    ShipName = salesOrderdto.ShipName,
                    ShipState = salesOrderdto.ShipState,
                    ShipStreet = salesOrderdto.ShipStreet,
                    ShipZipCode = salesOrderdto.ShipZipCode
                };

                var addedOrder = _context.SalesOrders.Add(salesOrder);

                await _context.SaveChangesAsync();


                try
                {
                    var cartItems = await CartRepository.GetCartItems(user.Cart.Id);

                    foreach (var cartItem in cartItems)
                    {
                        var orderDetail = new OrderDetail()
                        {
                            Product = cartItem.Product,
                            Quantity = cartItem.Quantity,
                            SalesOrder = addedOrder.Entity,
                            UnitPrice = cartItem.Product.Price
                        };

                        _context.OrderDetails.Add(orderDetail);
                    }

                    await _context.SaveChangesAsync();


                    //clean up users cart
                    user.Cart.TotalPrice = 0;
                    _context.Entry(user.Cart).State = EntityState.Modified;

                    _context.CartItems.RemoveRange(cartItems);


                    await _context.SaveChangesAsync();
                }
                catch (Exception ex)
                {
                    _context.SalesOrders.Remove(addedOrder.Entity);
                    await _context.SaveChangesAsync();
                    _logger.LogError($"Error while creating the order details or while cleaning up the users cart for order: {addedOrder.Entity.Id}. Error: " + ex.ToString());
                    throw;
                }


                SalesOrderDTO salesOrderDTO = new SalesOrderDTO()
                {
                    Id = addedOrder.Entity.Id
                };

                return salesOrderDTO;
            }
            catch (Exception ex)
            {
                _logger.LogError("Error while adding new Order. Error: " + ex.ToString());
                throw;
            }
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
                var order = await _context.SalesOrders.Include(x => x.Customer).Include(x=> x.Customer.ApplicationUser).FirstOrDefaultAsync(x => x.Id == id);
                if (order == null) { return NotFound(); }

                var orderItems = await _context.OrderDetails.Include(o => o.Product).Where(x => x.SalesOrder.Id == order.Id).ToListAsync();

                var invoice = new GeneratePDFInvoice(order, orderItems);

                var stream = new MemoryStream();
                invoice.GeneratePdf(stream);
                stream.Position = 0;

                return File(stream, "application/pdf", $"invoice_{id}.pdf");
            }
            catch (Exception ex)
            {
                _logger.LogError("Error while creating invoice pdf. Error: " + ex.ToString());
                return BadRequest("Could not create the invoice.");
                throw;
            }
        }
    }
}
