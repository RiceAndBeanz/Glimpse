using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using WebServices.Models;
using Glimpse.Core.Services.General;

namespace WebServices.Controllers
{
    public class AuthenticationController : ApiController
    {
        private GlimpseDbContext db = new GlimpseDbContext();

        [Route("authenticate")]
        public IHttpActionResult Authenticate(Vendor vendor)
        {
            Vendor vendorDb = db.Vendors.FirstOrDefault(e => e.Email.ToLower().Equals(vendor.Email.ToLower()));

            var hashedEnteredPassword = Cryptography.HashPassword(vendor.Password, vendorDb.Salt);
            
            if (!(vendorDb.Password == hashedEnteredPassword))
            {
                return Unauthorized();                
            }
            return Ok(new { success = true });
        }
    }
}
