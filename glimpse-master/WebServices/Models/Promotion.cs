
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebServices.Models
{
    public class Promotion
    {
        public Promotion()
        {
            PromotionImages = new List<PromotionImage>();
        }

        [Key]
        public int PromotionId { get; set; }

        public int VendorId { get; set; }

        public Vendor Vendor { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public Categories Category { get; set; }

        public DateTime PromotionStartDate { get; set; }

        public DateTime PromotionEndDate { get; set; }
 
        public string PromotionImageURL { get; set;}

        [NotMapped]
        public bool RequestFromWeb { get; set; }

        [NotMapped]
        public byte[] PromotionImage { get; set; }

        public virtual ICollection<PromotionImage> PromotionImages { get; set; }

    }

    public enum Categories
    {
        Footwear,
        Electronics,
        Jewellery,
        Restaurants,
        Services,
        Apparel
    }
   
}