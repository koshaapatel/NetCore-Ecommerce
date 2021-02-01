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
    public class ProductsController : ControllerBase
    {
        private readonly AppDbContext appDbContext;
        private readonly ILogger logger;

        public ProductsController(ILogger<ProductsController> logger, AppDbContext appDbContext)
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
                return new Response(appDbContext.Products.ToListAsync().Result);
            }
            catch (Exception ex)
            {
                logger.LogError(ex.ToString());
                return new Response(null, 404, ex.Message);
            }
        }

        // GET: api/Products/5
        [HttpGet("{id}")]
        public ActionResult<Response> GetProduct([FromRoute] int id)
        {
            try
            {
                return new Response(appDbContext.Products.SingleOrDefaultAsync(p => p.Id == id).Result);

            }
            catch (Exception ex)
            {
                logger.LogError(ex.ToString());
                return new Response(null, 404, ex.Message);
            }

        }

        // PUT: api/Products/5
        [HttpPut("{id}")]
        public ActionResult<Response> PutProduct([FromRoute] int id, [FromBody] Product product)
        {
            try
            {

                if (!string.IsNullOrEmpty(HttpContext.Session.GetString("is_login")))
                {
                    var oldProduct = appDbContext.Products.SingleOrDefault(p => p.Id == id);
                    if (oldProduct != null)
                    {
                        // oldUser = user;
                        oldProduct.Name = product.Name;
                        oldProduct.Price= product.Price;
                        oldProduct.ShippingCost= product.ShippingCost;
                        oldProduct.Description= product.Description;
                       
                        appDbContext.SaveChanges();
                        return new Response(oldProduct,200,"Product Updated Successfully");
                    }
                    else
                    {
                        return new Response(null, 404, "No Data Found for Product");
                    }
                }
                else
                {
                    return new Response(null, 404, "Authentication Failure");
                }


            }
            catch (Exception ex)
            {
                logger.LogError(ex, product.ToString());
                return new Response(null, 404, ex.Message);
            }
        }

        // POST: api/Products
        [HttpPost]
        public ActionResult<Response> PostProduct([FromBody] Product product)
        {
            try
            {
                if (!string.IsNullOrEmpty(HttpContext.Session.GetString("is_login")))
                {

                    appDbContext.Products.Add(product);
                    appDbContext.SaveChangesAsync();
                    return new Response(product, 200, "Item Inserted Successfully");
                }
                else
                {
                    return new Response(null, 404, "Authentication Failure");
                }

            }
            catch (Exception ex)
            {
                logger.LogError(ex, product.ToString());
                return new Response(null, 404, ex.Message);
            }
        }

        // DELETE: api/Products/5
        [HttpDelete("{id}")]
        public ActionResult<Response> DeleteProduct([FromRoute] int id)
        {
            try
            {
                if (!string.IsNullOrEmpty(HttpContext.Session.GetString("is_login")))
                {
                    var product = appDbContext.Products.SingleOrDefaultAsync(p => p.Id == id);
                    if (product.Result != null)
                    {
                        appDbContext.Products.Remove(product.Result);
                        appDbContext.SaveChangesAsync();
                        return new Response(null,200,"Product Deleted Successfully");
                    }
                    else
                    {
                        return new Response(null, 404, "No Data Found for Product");
                    }
                }
                else
                {
                    return new Response(null, 404, "Authentication Failure");
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