using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using WebServices.Models;
using Serilog;

namespace WebServices.Controllers
{
    public class UsersController : ApiController
    {
        private GlimpseDbContext db = new GlimpseDbContext();

        // GET: api/Users
        public IQueryable<User> GetUsers()
        {
            Log.Information("Getting all users");
            return db.Users;
        }

        // GET: api/Users/5
        [ResponseType(typeof(User))]
        public IHttpActionResult GetUser(int id)
        {
            Log.Information("Attemping to get user with id: {@id}", id);
            User user = db.Users.Find(id);
            if (user == null)
            {
                Log.Error("Could not find user with id: {@id}", id);
                return NotFound();
            }

            Log.Information("Found user with id: {@id}", id);
            return Ok(user);
        }


        // GET: api/Users/Search/lala@gmail.com/
        //trailing slash is important or else 404 error
        [Route("api/Users/Search/{email}/")]
        [ResponseType(typeof(User))]
        public IHttpActionResult GetUser(string email)
        {
            Log.Information("Attemping to get user with email: {@email}", email);
            //for most email providers, upper case is the same as lower
            User user = db.Users.FirstOrDefault(e=> e.Email.ToLower().Equals(email.ToLower()));
            if (user == null)
            {
                Log.Error("Could not find user with email: {@email}", email);
                return Ok();
            }

            Log.Information("Found user with email: {@email}", email);
            return Ok(user);
        }


        // PUT: api/Users/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutUser(int id, User user)
        {
            Log.Information("Attempting to update user: {@user} with id {@id}",user,id);
            if (!ModelState.IsValid)
            {
                Log.Error("Invalid model state for user: {@user} with id: {@id}", user, id);
                return BadRequest(ModelState);
            }

            if (id != user.UserId)
            {
                Log.Error("Id: {@id} is the incorrect id for user id: {@user}", id, user);
                return BadRequest();
            }

            db.Entry(user).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UserExists(id))
                {
                    Log.Error("User with id: {@id} does not exist!", id);
                    return NotFound();
                }
                else
                {
                    Log.Error("Update operation has failed for user with id: {@id}", id);
                    throw;
                }
            }
            Log.Information("User with id: {@id} has been updated!", id);
            return StatusCode(HttpStatusCode.NoContent);
        }

        // POST: api/Users
        [ResponseType(typeof(User))]
        public IHttpActionResult PostUser(User user)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Users.Add(user);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = user.UserId }, user);
        }

        // DELETE: api/Users/5
        [ResponseType(typeof(User))]
        public IHttpActionResult DeleteUser(int id)
        {
            Log.Information("Attemping to delete user with id: {@id}", id);
            User user = db.Users.Find(id);
            if (user == null)
            {
                Log.Error("User with id: {@id} does not exist!", id);
                return NotFound();
            }

            db.Users.Remove(user);
            db.SaveChanges();

            Log.Information("User with id: {@id} has been deleted.", id);
            return Ok(user);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool UserExists(int id)
        {
            return db.Users.Count(e => e.UserId == id) > 0;
        }
    }
}