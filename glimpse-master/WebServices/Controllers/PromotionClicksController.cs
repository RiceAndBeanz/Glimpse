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
    public class PromotionClicksController : ApiController
    {
        private GlimpseDbContext db = new GlimpseDbContext();

        // GET: api/PromotionClicks
        public IQueryable<PromotionClick> GetPromotionClicks()
        {
            Log.Information("Getting all promotion clicks");
            return db.PromotionClicks;
        }

        // GET: api/PromotionClicks/5
        [ResponseType(typeof(PromotionClick))]
        public IHttpActionResult GetPromotionClick(int id)
        {
            Log.Information("Attemping to get promotion clicks for id: {@id}", id);
            PromotionClick promotionClick = db.PromotionClicks.Find(id);
            if (promotionClick == null)
            {
                Log.Error("Could not find promotion clicks for id: {@id}", id);
                return NotFound();
            }

            Log.Information("Found promotion clicks for id: {@id}", id);
            return Ok(promotionClick);
        }

        // PUT: api/PromotionClicks/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutPromotionClick(int id, PromotionClick promotionClick)
        {
            Log.Information("Attempting to update promotion clicks for id: {@id}", id);
            if (!ModelState.IsValid)
            {
                Log.Error("Invalid model state for promotion clicks with id: {@id}", id);
                return BadRequest(ModelState);
            }

            if (id != promotionClick.PromotionClickId)
            {
                Log.Error("Id: {@id} is the incorrect id for promotion clicks id: {@promotionClick}", id, promotionClick.PromotionClickId);
                return BadRequest();
            }

            db.Entry(promotionClick).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PromotionClickExists(id))
                {
                    Log.Error("Promotion clicks with id: {@id} does not exist!", id);
                    return NotFound();
                }
                else
                {
                    Log.Error("Update operation has failed for promotion clicks with id: {@id}", id);
                    throw;
                }
            }

            Log.Information("Promotion clicks with id: {@id} has been updated!", id);
            return StatusCode(HttpStatusCode.NoContent);
        }

        // POST: api/PromotionClicks
        [ResponseType(typeof(PromotionClick))]
        public IHttpActionResult PostPromotionClick(PromotionClick promotionClick)
        {
            Log.Information("Attempting to add promotion clicks for id: {@promotionClick}", promotionClick.PromotionClickId);
            if (!ModelState.IsValid)
            {
                Log.Error("Invalid model state for promotion clicks with id: {@id}", promotionClick.PromotionClickId);
                return BadRequest(ModelState);
            }

            db.PromotionClicks.Add(promotionClick);
            db.SaveChanges();

            Log.Information("Promotion click with id: {@promotionClick} has been added to the database!", promotionClick.PromotionClickId);
            return CreatedAtRoute("DefaultApi", new { id = promotionClick.PromotionClickId }, promotionClick);
        }

        // DELETE: api/PromotionClicks/5
        [ResponseType(typeof(PromotionClick))]
        public IHttpActionResult DeletePromotionClick(int id)
        {
            Log.Information("Attemping to delete promotion click with id: {@id}", id);
            PromotionClick promotionClick = db.PromotionClicks.Find(id);
            if (promotionClick == null)
            {
                Log.Error("Promotion click with id: {@id} does not exist!", id);
                return NotFound();
            }

            db.PromotionClicks.Remove(promotionClick);
            db.SaveChanges();

            Log.Information("Promotion click with id: {@id} has been deleted.", id);
            return Ok(promotionClick);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool PromotionClickExists(int id)
        {
            return db.PromotionClicks.Count(e => e.PromotionClickId == id) > 0;
        }
    }
}