using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NetcoreProject2.Models;
using Microsoft.Extensions.Logging;
using NetcoreProject2.Utils;

namespace NetcoreProject2.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CartsController : ControllerBase
    {
        private readonly AppDbContext appDbContext;
        private readonly ILogger logger;

        public CartsController(ILogger<CartsController> logger, AppDbContext appDbContext)
        {
            this.logger = logger;
            this.appDbContext = appDbContext;
        }

        // GET: api/Carts
        [HttpGet]
        public ActionResult<Response> GetCarts()
        {
            try
            {
                int id = string.IsNullOrEmpty(HttpContext.Session.GetString("userid")) ? 1 : int.Parse(HttpContext.Session.GetString("userid"));
                return new Response(from cart in appDbContext.Carts.ToListAsync().Result
                                    where cart.UserId == id
                                    select cart);
            }
            catch (Exception ex)
            {
                logger.LogError(ex.ToString());
                return new Response(null, 404, ex.Message);
            }
        }

        // GET: api/Carts/5
        [HttpGet("{id}")]
        public ActionResult<Response> GetCart([FromRoute] int id)
        {
            try
            {
                int userid = string.IsNullOrEmpty(HttpContext.Session.GetString("userid")) ? 1 : int.Parse(HttpContext.Session.GetString("userid"));
                var cartList = from cart in appDbContext.Carts.ToListAsync().Result
                               where cart.UserId == userid && cart.Id == id
                               select cart;
                if (cartList.Count<Cart>() > 0)
                {
                    return new Response(cartList);
                }
                else
                {
                    return new Response(null, 404, "No data found for user");
                }

            }
            catch (Exception ex)
            {
                logger.LogError(ex.ToString());
                return new Response(null, 404, ex.Message);
            }
        }

        // PUT: api/Carts/5
        [HttpPut("{id}")]
        public ActionResult<Response> PutCart([FromRoute] int id, [FromBody] Cart cart)
        {
            try
            {
                int userid = string.IsNullOrEmpty(HttpContext.Session.GetString("userid")) ? 1 : int.Parse(HttpContext.Session.GetString("userid"));
                var oldCart = appDbContext.Carts.SingleOrDefault(p => p.Id == id && p.UserId == userid);
                if (oldCart != null)
                {
                    oldCart.ProductId = cart.ProductId;
                    oldCart.Quantity = cart.Quantity;
                    oldCart.Price = cart.Price;
                    appDbContext.SaveChangesAsync();
                    return new Response(oldCart, 200, "Updated Successfully");
                }
                else
                {
                    return new Response(null, 400, "No Data Found for Cart");
                }
            }
            catch (Exception ex)
            {
                logger.LogError(ex, cart.ToString());
                return new Response(null, 404, ex.Message);
            }
        }

        // POST: api/Carts
        [HttpPost]
        public ActionResult<Response> PostCart([FromBody] Cart cart)
        {
            try
            {
                int userid = string.IsNullOrEmpty(HttpContext.Session.GetString("userid")) ? 1 : int.Parse(HttpContext.Session.GetString("userid"));
                cart.UserId = userid;
                appDbContext.Carts.Add(cart);
                appDbContext.SaveChangesAsync();
                return new Response(cart);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, cart.ToString());
                return new Response(null, 404, ex.Message);
            }
        }

        // DELETE: api/Carts/5
        [HttpDelete("{id}")]
        public ActionResult<Response> DeleteCart([FromRoute] int id)
        {
            try
            {

                int userid = string.IsNullOrEmpty(HttpContext.Session.GetString("userid")) ? 1 : int.Parse(HttpContext.Session.GetString("userid"));
                var oldCart = appDbContext.Carts.SingleOrDefault(p => p.Id == id && p.UserId == userid);
                if (oldCart != null)
                {
                    appDbContext.Carts.Remove(oldCart);
                    appDbContext.SaveChangesAsync();
                    return new Response(null, 200, "Deleted Successfully");
                }
                else
                {
                    return new Response(null, 400, "No Data Found for Cart");
                }
            }
            catch (Exception ex)
            {
                logger.LogError(ex.ToString());
                return new Response(null, 404, ex.Message);
            }

        }
    }
}