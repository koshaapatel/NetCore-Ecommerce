using System;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using NetcoreProject2.Models;
using NetcoreProject2.Utils;

namespace Netcore_Project2.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly AppDbContext appDbContext;
        private readonly ILogger logger;

        public UserController(ILogger<UserController> logger, AppDbContext appDbContext)
        {
            this.logger = logger;
            this.appDbContext = appDbContext;
        }

        // GET: api/User
        [HttpGet]
        public ActionResult<Response> Get()
        {
            try
            {
                if (!string.IsNullOrEmpty(HttpContext.Session.GetString("is_login")))
                {
                    return new Response(appDbContext.Users.ToListAsync().Result);
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

        // GET: api/User/5
        [HttpGet("{id}")]
        public ActionResult<Response> Get(int id)
        {
            try
            {
                if (!string.IsNullOrEmpty(HttpContext.Session.GetString("is_login")))
                {
                    return new Response(appDbContext.Users.SingleOrDefaultAsync(p => p.Id == id).Result);
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

        // POST: api/User
        [HttpPost]
        public ActionResult<Response> Post([FromBody] User user)
        {
            try
            {
                   
                    user.Password = CryptoEngine.Encrypt(user.Password, "sblw-3hn8-sqoy19");
                    appDbContext.Users.Add(user);
                    appDbContext.SaveChangesAsync();
                    user.Password = CryptoEngine.Decrypt(user.Password, "sblw-3hn8-sqoy19");
                    return new Response(user, 200, "User Inserted Successfully");
           

            }
            catch (Exception ex)
            {
                logger.LogError(ex, user.ToString());
                return new Response(null, 404, ex.Message);
            }
        }

        // PUT: api/User/5
        [HttpPut("{id}")]
        public ActionResult<Response> Put(int id, [FromBody] User user)
        {
            try
            {

                if (!string.IsNullOrEmpty(HttpContext.Session.GetString("is_login")))
                {
                    var oldUser = appDbContext.Users.SingleOrDefault(p => p.Id == id);
                    if (oldUser != null)
                    {
                        // oldUser = user;
                        oldUser.Email = user.Email;
                        oldUser.Username = user.Username;
                        oldUser.AddressLine1 = user.AddressLine1;
                        oldUser.AddressLine2 = user.AddressLine2;
                        oldUser.City = user.City;
                        oldUser.State = user.State;
                        oldUser.Country = user.Country;
                        appDbContext.SaveChanges();
                        return new Response(oldUser);
                    }
                    else
                    {
                        return new Response(null, 404, "No Data Found for User");
                    }
                }
                else
                {
                    return new Response(null, 404, "Authentication Failure");
                }


            }
            catch (Exception ex)
            {
                logger.LogError(ex, user.ToString());
                return new Response(null, 404, ex.Message);
            }

        }

        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}")]
        public ActionResult<Response> Delete(int id)
        {
            try
            {
                if (!string.IsNullOrEmpty(HttpContext.Session.GetString("is_login")))
                {
                    var user = appDbContext.Users.SingleOrDefaultAsync(p => p.Id == id);
                    if (user.Result != null)
                    {
                        appDbContext.Users.Remove(user.Result);
                        appDbContext.SaveChangesAsync();
                        return new Response(null);
                    }
                    else
                    {
                        return new Response(null, 404, "No Data Found for User");
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
