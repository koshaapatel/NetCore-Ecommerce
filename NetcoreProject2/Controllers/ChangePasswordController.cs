using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using NetcoreProject2.Models;
using NetcoreProject2.Utils;

namespace NetcoreProject2.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ChangePasswordController : ControllerBase
    {
        private readonly AppDbContext appDbContext;
        private readonly ILogger logger;

        public ChangePasswordController(
          ILogger<ChangePasswordController> logger,
          AppDbContext appDbContext
       )
        {
            this.logger = logger;
            this.appDbContext = appDbContext;
        }

        public ActionResult<Response> Put([FromBody] User user)
        {
            try
            {

                if (HttpContext.Session.GetString("is_login").ToLower().Equals("true"))
                {
                    var oldUser = appDbContext.Users.SingleOrDefault(p => p.Id == int.Parse(HttpContext.Session.GetString("userid")));
                    if (oldUser != null)
                    {
                        oldUser.Password = CryptoEngine.Encrypt(user.Password, "sblw-3hn8-sqoy19");
                        appDbContext.SaveChanges();
                        return new Response(oldUser);
                    }
                    else
                    {
                        return new Response(null, 404, "No User Found");
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
    }
}