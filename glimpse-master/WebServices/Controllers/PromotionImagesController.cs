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
using WebServices.Helpers;
using WebServices.Models;
using Serilog;

namespace WebServices.Controllers
{
    public class PromotionImagesController : ApiController
    {
        private GlimpseDbContext db = new GlimpseDbContext();

        // GET: api/PromotionImages
        public IQueryable<PromotionImage> GetPromotionImages()
        {
            Log.Information("Getting all promotion images");
            return db.PromotionImages;
        }

        // GET: api/PromotionImages/5
        [ResponseType(typeof(PromotionImage))]
        public IHttpActionResult GetPromotionImage(int id)
        {
            Log.Information("Attemping to get promotion image with id: {@id}", id);
            PromotionImage promotionImage = db.PromotionImages.Find(id);
            if (promotionImage == null)
            {
                Log.Error("Could not find promotion image with id: {@id}", id);
                return NotFound();
            }

            Log.Information("Found promotion image with id: {@id}", id);
            return Ok(promotionImage);
        }

        // PUT: api/PromotionImages/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutPromotionImage(int id, PromotionImage promotionImage)
        {
            Log.Information("Attempting to update promotion image with id: {@id}", id);
            if (!ModelState.IsValid)
            {
                Log.Error("Invalid model state for promotion image with id: {@id}", id);
                return BadRequest(ModelState);
            }

            if (id != promotionImage.PromotionImageId)
            {
                Log.Error("Id: {@id} is the incorrect id for promotion image id: {@promotionImage}", id, promotionImage.PromotionImageId);
                return BadRequest();
            }

            db.Entry(promotionImage).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PromotionImageExists(id))
                {
                    Log.Error("Promotion Image with id: {@id} does not exist!", id);
                    return NotFound();
                }
                else
                {
                    Log.Error("Update operation has failed for promotion image with id: {@id}", id);
                    throw;
                }
            }

            Log.Information("Promotion image with id: {@id} has been updated!", id);
            return StatusCode(HttpStatusCode.NoContent);
        }

        // POST: api/PromotionImages
        [ResponseType(typeof(PromotionImage))]
        public IHttpActionResult PostPromotionImage(PromotionImage promotionImage)
        {
            BlobHelper bh = new BlobHelper("glimpseimages", "XHIr8SaKFci88NT8Z+abpJaH1FeLC4Zq6ZRaIkaAJQc+N/1nwTqGPzDLdNZXGqcLNg+mK7ugGW3PyJsYU2gB7w==", "imagestorage");
            Log.Information("Attempting to add promotion image with id: {@promotionImage}", promotionImage.PromotionImageId);
            bh.UploadFromByteArray(promotionImage.Image, promotionImage.ImageURL);

            if (!ModelState.IsValid)
            {
                Log.Error("Invalid model state for promotion image with id: {@id}", promotionImage.PromotionImageId);
                return BadRequest(ModelState);
            }

            db.PromotionImages.Add(promotionImage);
            db.SaveChanges();

            Log.Information("Promotion image with id: {@promotionImage} has been added to the database!",promotionImage.PromotionImageId);
            return CreatedAtRoute("DefaultApi", new { id = promotionImage.PromotionImageId }, promotionImage);
        }

        // DELETE: api/PromotionImages/5
        [ResponseType(typeof(PromotionImage))]
        public IHttpActionResult DeletePromotionImage(int id)
        {
            Log.Information("Attemping to delete promotion image with id: {@id}", id);
            PromotionImage promotionImage = db.PromotionImages.Find(id);
            if (promotionImage == null)
            {
                Log.Error("Promotion image with id: {@id} does not exist!", id);
                return NotFound();
            }

            db.PromotionImages.Remove(promotionImage);
            db.SaveChanges();

            Log.Information("Promotion image with id: {@id} has been deleted.", id);
            return Ok(promotionImage);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool PromotionImageExists(int id)
        {
            return db.PromotionImages.Count(e => e.PromotionImageId == id) > 0;
        }
    }
}