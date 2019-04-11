using System.Data.Entity;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Web.Http;
using System.Web.Http.Description;
using WebServices.Models;
using WebServices.Helpers;
using System;
using Serilog;

namespace WebServices.Controllers
{
    public class PromotionsController : ApiController
    {
        private GlimpseDbContext db = new GlimpseDbContext();

        private readonly BlobHelper bh = new BlobHelper("glimpseimages", "XHIr8SaKFci88NT8Z+abpJaH1FeLC4Zq6ZRaIkaAJQc+N/1nwTqGPzDLdNZXGqcLNg+mK7ugGW3PyJsYU2gB7w==", "imagestorage");

        // GET: api/Promotions
        public IQueryable<Promotion> GetPromotions(bool active = false, string keyword = "")
        {
            IQueryable<Promotion> listOfPromos = db.Promotions;
            if (active)
            {
                Log.Information("Attempting to get all active promotions");
                listOfPromos = listOfPromos.Where(e => e.PromotionStartDate.CompareTo(DateTime.Now) <= 0 && e.PromotionEndDate.CompareTo(DateTime.Now) >= 0);
            }

            if (!string.IsNullOrEmpty(keyword))
            {
                Log.Information("Getting all promotions with keyword: {@keyword} in title or description: " + keyword);
                listOfPromos = listOfPromos.Where(promo => promo.Title.Contains(keyword) || promo.Description.Contains(keyword));
            }

            Log.Information("Returning list of promotions");
            return listOfPromos;
        }

        // GET: api/Promotions/5
        [ResponseType(typeof(Promotion))]
        public IHttpActionResult GetPromotion(int id)
        {
            Promotion promotion = db.Promotions.Find(id);
            if (promotion == null)
            {
                Log.Error("Could not find promotion with id: {@id}", id);
                return NotFound();
            }

            Log.Information("Found promotion with id: {@id}", id);
            return Ok(promotion);
        }

        // GET: api/Vendors/5/promotions
        [ResponseType(typeof(Vendor))]
        [Route("api/Promotions/{id}/promotionclicks")]
        public IHttpActionResult GetVendorPromotions(int id)
        {
            Log.Information("Attemping to get vendor promotion(s) with id: {@id}", id);
            List<PromotionClick> promotionClicksOfPromotion = db.PromotionClicks.Where(promoClick => promoClick.PromotionId == id).ToList();
            /*if (vendor == null)
            {
                return NotFound();
            } */

            Log.Information("Returning vendor promotions found with id: {@id}", id);
            return Ok(promotionClicksOfPromotion);
        }

        // GET: api/Promotions/5/promotionimages
        [ResponseType(typeof(Vendor))]
        [Route("api/Promotions/{id}/promotionimages")]
        public IHttpActionResult GetVendorPromotionsImages(int id)
        {
            List<PromotionImage> promotionImagesOfPromotion = db.PromotionImages.Where(promoImage => promoImage.PromotionId == id).ToList();
            /*if (vendor == null)
            {
                return NotFound();
            } */

            return Ok(promotionImagesOfPromotion);
        }

        // GET: api/Vendors/5/promotions
        [ResponseType(typeof(Vendor))]
        [Route("api/Promotions/filter/{filterName}")]
        public IHttpActionResult GetVendorPromotions(Categories filterName)
        {
            Log.Information("Attemping to get vendor promotion(s) by category: {@filterName}", filterName);
            List<Promotion> promotionsFiltered = db.Promotions.Where(promo => promo.Category == filterName).ToList();
            /*if (vendor == null)
            {
                return NotFound();
            } */

            Log.Information("Returning vendor promotions that was found by cateogory: {@filterName}", filterName);
            return Ok(promotionsFiltered);
        }

        // GET: api/Vendors/5/promotions
        [ResponseType(typeof(Vendor))]
        //[Route("api/Promotions/Search/{filterName}")]
        public IHttpActionResult Search(string keyword)
        {
            List<Promotion> promotionsFiltered = db.Promotions.Where(promo => promo.Title.Contains(keyword) || promo.Description.Contains(keyword)).ToList();

            return Ok(promotionsFiltered);
        }


        // PUT: api/Promotions/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutPromotion(int id, Promotion promotion)
        {
            Log.Information("Attempting to update promotion for promotion: {@promotion} with id {@id}", promotion.Title, id);
            if (!ModelState.IsValid)
            {
                Log.Error("Invalid model state for promotion: {@promotion} with id: {@id}", promotion.Title, id);
                return BadRequest(ModelState);
            }

            bh.UploadFromByteArray(promotion.PromotionImage, promotion.PromotionImageURL);

            if (id != promotion.PromotionId)
            {
                Log.Error("Id: {@id} is the incorrect id for promotion: {@promotion}", id, promotion.Title);
                return BadRequest();
            }

            db.Entry(promotion).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PromotionExists(id))
                {
                    Log.Error("Promotion with id: {@id} does not exist!", id);
                    return NotFound();
                }
                else
                {
                    Log.Error("Update operation has failed for promotion with id: {@id}", id);
                    throw;
                }
            }
            Log.Information("Promotion with id: {@id} has been updated!", id);
            return StatusCode(HttpStatusCode.NoContent);
        }

        // POST: api/Promotions
        [ResponseType(typeof(Promotion))]
        public IHttpActionResult PostPromotion(Promotion promotion)
        {

            BlobHelper bh = new BlobHelper("glimpseimages", "XHIr8SaKFci88NT8Z+abpJaH1FeLC4Zq6ZRaIkaAJQc+N/1nwTqGPzDLdNZXGqcLNg+mK7ugGW3PyJsYU2gB7w==", "imagestorage");
            Log.Information("Attempting to add promotion: {@promotion}", promotion.Title);
            bh.UploadFromByteArray(promotion.PromotionImage, promotion.PromotionImageURL);

            if(promotion.RequestFromWeb == true)
            {
                Log.Information("Attemping to post web promotion {@promotion}", promotion.PromotionId);
                int size = promotion.PromotionImages.Count;
                Log.Information("Size of Promotion Images: {@size}", size);
                for (int i = 0; i < size; i++)
                {
                    string response = bh.UploadFromByteArray(promotion.PromotionImages.ElementAt(i).Image, promotion.PromotionImages.ElementAt(i).ImageURL);
                    Log.Information("Testing this inside web loop for images");
                }
            }

            if (!ModelState.IsValid)
            {
                Log.Error("Invalid model state for promotion: {@promotion}", promotion.Title);
                return BadRequest(ModelState);
            }

            db.Promotions.Add(promotion);
            db.SaveChanges();

            Log.Information("Promotion: {@promotion} has been added to the database!", promotion.Title);
            return CreatedAtRoute("DefaultApi", new { id = promotion.PromotionId }, promotion);
        }

        // DELETE: api/Promotions/5
        [ResponseType(typeof(Promotion))]
        public IHttpActionResult DeletePromotion(int id)
        {
            Log.Information("Attemping to delete promotion with id: {@id}", id);
            Promotion promotion = db.Promotions.Find(id);
            if (promotion == null)
            {
                Log.Error("Promotion with id: {@id} does not exist!", id);
                return NotFound();
            }

            db.Promotions.Remove(promotion);
            db.SaveChanges();

            Log.Information("Promotion with id: {@id} has been deleted.", id);
            return Ok(promotion);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool PromotionExists(int id)
        {
            return db.Promotions.Count(e => e.PromotionId == id) > 0;
        }
    }
}