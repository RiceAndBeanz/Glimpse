using System;
using System.Collections.Generic;
using System.Linq;
using Android.Gms.Maps.Model;
using Android.Graphics;
using Com.Google.Maps.Android.Clustering;
using Glimpse.Core.Model;
using Glimpse.Droid.Helpers;


namespace Glimpse.Core.Helpers
{
    public class PromotionItem : Java.Lang.Object, IClusterItem
    {
        private PromotionWithLocation currentPromotion;
        public PromotionItem(List<PromotionWithLocation> promotionItems, double lat, double lng)
        {
            currentPromotion = promotionItems.LastOrDefault();
            Position = new LatLng(lat, lng);
        }

        /*
        public PromotionItem(double lat, double lng, string title, string description, int expirationDate, string companyName, Bitmap promotionImage, int promotionId)
        {
            Position = new LatLng(lat, lng);
            Title = title;
            Description = description;
            ExpirationDate = expirationDate;
            CompanyName = companyName;
            PromotionImage = promotionImage;
            PromotionId = promotionId;
        }*/

        public int PromotionId { get; set; }
        public LatLng Position { get; set; }

        public PromotionWithLocation CurrentPromotion
        {
            set { currentPromotion = value; }
            get { return currentPromotion; }
        }
    }
}
