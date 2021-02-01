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
    public class LoginController : ControllerBase
    {


        private readonly AppDbContext appDbContext;
        private readonly ILogger logger;

        public LoginController(
          ILogger<LoginController> logger,
          AppDbContext appDbContext
       )
        {
            this.logger = logger;
            this.appDbContext = appDbContext;
        }

        // POST: api/Login
        [HttpPost]
        public ActionResult<Response> Post([FromBody] User user)
        {
            try
            {
                var fetched_user = appDbContext.Users.SingleOrDefaultAsync(p => p.Email == user.Email);
                if (fetched_user.Result != null && user.Password == CryptoEngine.Decrypt(fetched_user.Result.Password, "sblw-3hn8-sqoy19")){
                    HttpContext.Session.SetString("is_login", "true");
                    HttpContext.Session.SetString("userid", fetched_user.Result.Id.ToString());
                    return new Response(null,200,"Login Successfully");
                }
                else {
                    return new Response(null, 200, "Login Failed");
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