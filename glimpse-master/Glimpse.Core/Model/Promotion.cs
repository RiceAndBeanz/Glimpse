
//using Plugin.Media.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Glimpse.Core.Model
{
    public class Promotion
    {
        public int PromotionId { get; set; }

       
        public int VendorId { get; set; }

        public Vendor Vendor { get; set; }

        private string _title;
        public string Title
        {
            get { return _title; }
            set { _title = value; }
        }

        private string _description;
        public string Description
        {
            get { return _description; }
            set { _description = value; }
        }

        public Categories Category { get; set; }

        public DateTime PromotionStartDate { get; set; }

        public DateTime PromotionEndDate { get; set; }

        public byte[] PromotionImage { get; set; }

        public string PromotionImageURL { get; set; }

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