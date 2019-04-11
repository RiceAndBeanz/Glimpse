using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Web.Http;
using System.Web.Http.Description;
using WebServices.Models;
using Serilog;
using WebServices.Utilities;
using System.Net.Mail;

namespace WebServices.Controllers
{
    public class VendorsController : ApiController
    {
        private GlimpseDbContext db = new GlimpseDbContext();

        // GET: api/Vendors
        public IQueryable<Vendor> GetVendors()
        {
            Log.Information("Getting all vendors");
            DbSet<Vendor> vendors = db.Vendors;
            //hiding password and salt from API
            foreach(var vendor in vendors)
            {
                vendor.Password = null;
                vendor.Salt = null;
            }
            return vendors;
        }

        // GET: api/Vendors/5
        [Route("api/Vendors/{id}")]
        [ResponseType(typeof(Vendor))]
        public IHttpActionResult GetVendor(int id)
        {
            Log.Information("Attemping to get vendor with id: {@id}", id);
            Vendor vendor = db.Vendors.Find(id);           

            if (vendor == null)
            {
                Log.Error("Could not find vendor with id: {@id}", id);
                return NotFound();
            }

            //hiding password and salt from API
            vendor.Password = null;
            vendor.Salt = null;

            Log.Information("Found vendor with id: {@id}", id);
            return Ok(vendor);
        }


        // GET: api/Vendors/Search/lala@gmail.com/
        //trailing slash is important or else 404 error
        [Route("api/Vendors/Search/{email}/")]
        [ResponseType(typeof(Vendor))]
        public IHttpActionResult GetVendor(string email)
        {
            Log.Information("Attemping to get vendor with email: {@email}", email);
            //for most email providers, upper case is the same as lower
            Vendor vendor = db.Vendors.FirstOrDefault(e => e.Email.ToLower().Equals(email.ToLower()));
            if (vendor == null)
            {
                Log.Error("Could not find vendor with email: {@email}", email);
                return Ok();
            }
            Log.Information("Found vendor with email: {@email}", email);
            //hiding password and salt from API
            vendor.Password = null;
            vendor.Salt = null;

            return Ok(vendor);
        }

        // GET: api/Vendors/5/promotions
        [ResponseType(typeof(Vendor))]
        [Route("api/Vendors/{id}/promotions")]
        public IHttpActionResult GetVendorPromotions(int id)
        {
            Log.Information("Getting vendor promotions from vendor id: {@id}", id);
            List<Promotion> promosOfVendor = db.Promotions.Where(promo => promo.VendorId == id).ToList();
            /*if (vendor == null)
            {
                return NotFound();
            } */
            Log.Information("Returning vendor promotions from vendor id: {@id}", id);
            return Ok(promosOfVendor);
        }

        // PUT: api/Vendors/5
        [ResponseType(typeof(void))]
        [HttpPut]
        [Route("api/Vendors/{id}")]
        public IHttpActionResult PutVendor(int id, Vendor vendor)
        {
            Log.Information("Attempting to update vendor: {@vendor} with id {@id}", vendor.CompanyName,id);
            if (!ModelState.IsValid)
            {
                Log.Error("Invalid model state for vendor: {@vendor} with id: {@id}", vendor.CompanyName, id);
                return BadRequest(ModelState);
            }

            if (id != vendor.VendorId)
            {
                Log.Error("Id: {@id} is the incorrect id for vendor id: {@vendor}", id, vendor.CompanyName);
                return BadRequest();
            }

            Vendor updateVendor = db.Vendors.FirstOrDefault(e => e.VendorId == (vendor.VendorId));
            updateVendor.Email = vendor.Email;
            updateVendor.Address = vendor.Address;
            updateVendor.Telephone = vendor.Telephone;
            updateVendor.Location = vendor.Location;
            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!VendorExists(id))
                {
                    Log.Error("Vendor with id: {@id} does not exist!", id);
                    return NotFound();
                }
                else
                {
                    Log.Error("Update operation has failed for vendor with id: {@id}", id);
                    throw;
                }
            }

            Log.Information("Vendor with id: {@id} has been updated!", id);
            return StatusCode(HttpStatusCode.NoContent);
        }

        /*
        // PUT: api/Vendors/5/promotions
        [ResponseType(typeof(void))]
        [Route("api/Vendors/{id}/promotions")]
        public IHttpActionResult PutCollectionVendor(int id, Vendor vendor)
        {
            if (vendor.Promotions.Count == 0)
            {
                vendor.CompanyName = "its empty";
            }
            else
            {
                vendor.CompanyName = vendor.Promotions.ElementAt(0).Description;
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != vendor.VendorId)
            {
                return BadRequest();
            }

            db.Entry(vendor.Promotions).State = EntityState.Modified;
            db.Entry(vendor).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!VendorExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return StatusCode(HttpStatusCode.NoContent);
        }
        */

        // POST: api/Vendors
        [ResponseType(typeof(Vendor))]
        public IHttpActionResult PostVendor(Vendor vendor)
        {
            Log.Information("Attempting to add vendor: {@vendor}",vendor.CompanyName);
            if (!ModelState.IsValid)
            {
                Log.Error("Invalid model state for vendor: {@vendor}", vendor.CompanyName);
                return BadRequest(ModelState);
            }
            if (vendor.RequestFromWeb == true)
            {
                var cryptoTuple = Utility.Cryptography.HashPassword(vendor.Password);
                vendor.Password = cryptoTuple.Item1;
                vendor.Salt = cryptoTuple.Item2;
            }
            db.Vendors.Add(vendor);
            db.SaveChanges();

            //Mail.SendEmail(vendor);

            Log.Information("Vendor: {@vendor} has been added to the database!",vendor.CompanyName);
            return CreatedAtRoute("DefaultApi", new { id = vendor.VendorId }, vendor);
        }

        // DELETE: api/Vendors/5
        [ResponseType(typeof(Vendor))]
        [HttpDelete]
        [Route("api/Vendors/{id}")]
        public IHttpActionResult DeleteVendor(int id)
        {
            Log.Information("Attemping to delete vendor with id: {@id}", id);
            Vendor vendor = db.Vendors.Find(id);
            if (vendor == null)
            {
                Log.Error("Vendor with id: {@id} does not exist!", id);
                return NotFound();
            }

            db.Vendors.Remove(vendor);
            db.SaveChanges();

            Log.Information("Vendor with id: {@id} has been deleted.", id);
            return Ok(vendor);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool VendorExists(int id)
        {
            return db.Vendors.Count(e => e.VendorId == id) > 0;
        }
    }
}