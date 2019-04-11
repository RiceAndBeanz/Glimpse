using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Claims;
using System.Web.Http;

namespace WebServices.Controllers
{
    public class HomeController : ApiController
    {
        [Authorize]
        [HttpGet]
        [Route("api/data/home")]
        public IHttpActionResult GetForAuthenticate()
        {
            var identity = (ClaimsIdentity)User.Identity;
            return Ok(identity.Name);
        }
    }
}