using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using NetcoreProject2.Models;
using NetcoreProject2.Utils;

namespace NetcoreProject2.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        private readonly AppDbContext appDbContext;
        private readonly ILogger logger;

        public OrdersController(
          ILogger<OrdersController> logger,
          AppDbContext appDbContext
       )
        {
            this.logger = logger;
            this.appDbContext = appDbContext;
        }

        // GET: api/Products
        [HttpGet]
        public ActionResult<Response> GetProduct()
        {
            try
            {
                int id = string.IsNullOrEmpty(HttpContext.Session.GetString("userid")) ? 1 : int.Parse(HttpContext.Session.GetString("userid"));
                var orders = from order in appDbContext.Orders.ToListAsync().Result
                             where order.UserId == id
                             select order;
                return new Response(orders);
            }
            catch (Exception ex)
            {
                logger.LogError(ex.ToString());
                return new Response(null, 404, ex.Message);
            }
        }

        // GET: api/Orders/5
        [HttpGet("{id}")]
        public ActionResult<Response> GetOrder([FromRoute] int id)
        {
            try
            {
                int userid = string.IsNullOrEmpty(HttpContext.Session.GetString("userid")) ? 1 : int.Parse(HttpContext.Session.GetString("userid"));
                Order order = appDbContext.Orders.SingleOrDefault(p => p.Id == id && p.UserId == userid);
                return new Response(order, 200, "Updated Successfully");
            }
            catch (Exception ex)
            {
                logger.LogError(ex.ToString());
                return new Response(null, 404, ex.Message);
            }
        }

        // PUT: api/Orders/5
        [HttpPut("{id}")]
        public ActionResult<Response> PutOrder([FromRoute] int id, [FromBody] Order order)
        {
            try
            {
                int userid;
                if (!string.IsNullOrEmpty(HttpContext.Session.GetString("userid")))
                {
                    userid = int.Parse(HttpContext.Session.GetString("userid"));
                    var oldOrder = appDbContext.Orders.SingleOrDefault(p => p.Id == id && p.UserId == userid);
                    if (oldOrder != null)
                    {
                        oldOrder.OrderDetails = order.OrderDetails;
                        appDbContext.SaveChangesAsync();
                        return new Response(oldOrder, 200, "Updated Successfully");
                    }
                    else
                    {
                        return new Response(null, 400, "No Data Found for Order");
                    }
                }
                else
                {
                    return new Response(null, 400, "Authentication Error");
                }



            }
            catch (Exception ex)
            {
                logger.LogError(ex.ToString());
                return new Response(null, 404, ex.Message);
            }
        }

        // POST: api/Orders
        [HttpPost]
        public ActionResult<Response> PostOrder([FromBody] Order order)
        {
            //try
            //{
                int userid;
                if (!string.IsNullOrEmpty(HttpContext.Session.GetString("userid")))
                {
                    userid = int.Parse(HttpContext.Session.GetString("userid"));
                    var cartItems = appDbContext.Carts.ToListAsync().Result;
                    var filtered_cartItems = from cart in cartItems
                                             where cart.UserId == userid
                                             select cart;

                    if (filtered_cartItems.ToList().Count <= 0)
                    {

                        return new Response(null, 404, "Nothing in Cart");
                    }
                    else
                    {
                        order = new Order();
                    order.OrderDetails = new List<OrderDetail>();
                    order.Id = 2;

                        foreach (var cartItem in filtered_cartItems)
                        {
                            OrderDetail Detail = new OrderDetail();
                            Detail.ProductId = cartItem.ProductId;
                            var Product = appDbContext.Products.FirstOrDefaultAsync(i => i.Id == Detail.ProductId).Result;
                            Detail.Price = Product.Price;
                            Detail.Quantity = cartItem.Quantity;
                            Detail.TotalPrice = cartItem.Quantity * cartItem.Product.Price;
                            
                            //order.OrderDetails.Add(Detail);
                            appDbContext.OrderDetails.Add(Detail);
                            
                        }
                        order.UserId = userid;
                        
                        appDbContext.Orders.Add(order);
                        appDbContext.SaveChanges();
                        return new Response(order);
                    }
                    
                }
                else
                {
                    return new Response(null, 400, "Authentication Error");
                }

            //}
            //catch (Exception ex)
            //{
            //    logger.LogError(ex, order.ToString());
            //    return new Response(null, 404, ex.Message);
            //}
        }

        // DELETE: api/Orders/5
        [HttpDelete("{id}")]
        public ActionResult<Response> DeleteOrder([FromRoute] int id)
        {
            try
            {
                int userid;
                if (!string.IsNullOrEmpty(HttpContext.Session.GetString("userid")))
                {
                    userid = int.Parse(HttpContext.Session.GetString("userid"));
                    var oldOrder = appDbContext.Orders.SingleOrDefault(p => p.Id == id && p.UserId == userid);
                    if (oldOrder != null)
                    {
                        appDbContext.Orders.Remove(oldOrder);
                        appDbContext.SaveChangesAsync();
                        return new Response(null, 200, "Deleted Successfully");
                    }
                    else
                    {
                        return new Response(null, 400, "No Data Found for Cart");
                    }
                }
                else {
                    return new Response(null, 400, "Authentication Error");
                }
            }
            catch (Exception ex)
            {
                logger.LogError(ex,"");
                return new Response(null, 404, ex.Message);
            }

        }

    }

}