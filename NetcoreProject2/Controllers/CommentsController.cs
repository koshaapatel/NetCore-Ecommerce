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
    public class CommentsController : ControllerBase
    {
        private readonly AppDbContext appDbContext;
        private readonly ILogger logger;

        public CommentsController(AppDbContext context)
        {
            appDbContext = context;
        }

        // GET: api/Comments
        [HttpGet]
        public ActionResult<Response> GetComments()
        {
            var comment = appDbContext.Comments;

            return new Response(comment, 200, "Successfully");
        }

        // GET: api/Comments/5
        [HttpGet("{id}")]
        public  ActionResult<Response> GetComment([FromRoute] int id)
        {

            var comment =  appDbContext.Comments.FindAsync(id);

            return new Response(comment, 200, "Successfully");
        }

        // PUT: api/Comments/5
        [HttpPut("{id}")]
        public ActionResult<Response> PutComment([FromRoute] int id, [FromBody] Comment comment)
        {

            try
            {
                int userid = string.IsNullOrEmpty(HttpContext.Session.GetString("userid")) ? 1 : int.Parse(HttpContext.Session.GetString("userid"));
                var oldCart = appDbContext.Comments.SingleOrDefault(p => p.Id == id && p.UserId == userid);
                if (oldCart != null)
                {
                    oldCart.Rating = comment.Rating;
                   
                    appDbContext.SaveChangesAsync();
                    return new Response(oldCart, 200, "Updated Successfully");
                }
                else
                {
                    return new Response(null, 400, "No Data Found for Comment");
                }
            }
            catch (Exception ex)
            {
                logger.LogError(ex, comment.ToString());
                return new Response(null, 404, ex.Message);
            }

        }

        // POST: api/Comments
        [HttpPost]
        public ActionResult<Response> PostComment([FromBody] Comment comment)
        {
              
            try
            {
                int userid;
                if (!string.IsNullOrEmpty(HttpContext.Session.GetString("userid")))
                {
                    userid = int.Parse(HttpContext.Session.GetString("userid"));
                    var Orders = appDbContext.Orders.ToListAsync().Result;
                    var filtered_orders = from Order in Orders
                                          where Order.UserId == userid
                                          select Order;
                    var orders = appDbContext.Orders.ToListAsync().Result;



                    if (filtered_orders.ToList().Count <= 0)
                    {

                        return new Response(null, 404, "You do not have any order");

                    }
                    else
                    {
                        appDbContext.Comments.Add(comment);
                        appDbContext.SaveChangesAsync();
                        return new Response(comment);

                    }
                }
                else {
                    return new Response(null, 400, "Authentication error");
                }
                
            }
            catch (Exception ex)
            {
                logger.LogError(ex, comment.ToString());
                return new Response(null, 404, ex.Message);

            }
            
        }

        // DELETE: api/Comments/5
        [HttpDelete("{id}")]
        public ActionResult<Response> DeleteComment([FromRoute] int id)
        {
           
            Comment comment =  appDbContext.Comments.FindAsync(id).Result;
            if (comment == null)
            {
                return new Response(null, 404,"Failed");
            }

            appDbContext.Comments.Remove(comment);
            appDbContext.SaveChangesAsync();

            return new Response(null);
        }

        private bool CommentExists(int id)
        {
            return appDbContext.Comments.Any(e => e.Id == id);
        }
    }
}