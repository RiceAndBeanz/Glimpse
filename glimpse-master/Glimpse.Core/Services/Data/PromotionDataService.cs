using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Android.OS;
using Glimpse.Core.Contracts.Repository;
using Glimpse.Core.Contracts.Services;
using Glimpse.Core.Model;
using Glimpse.Core.BlobClient;


namespace Glimpse.Core.Services.Data
{
    public class PromotionDataService : IPromotionDataService
    {
        private readonly IPromotionRepository promotionRepository;
        private readonly IVendorRepository vendorRepository;

        public PromotionDataService(IPromotionRepository promotionRepository, IVendorRepository vendorRepositor)
        {
            this.promotionRepository = promotionRepository;
            this.vendorRepository = vendorRepositor;
        }

        public async Task<List<Promotion>> GetPromotion(int id)
        {
            return await promotionRepository.GetPromotion(id);
        }

        public async Task<List<Promotion>> GetPromotions()
        {
            return await promotionRepository.GetPromotions();
        }

        public async Task<List<Promotion>> GetPromotionsByCategory(Categories category)
        {
            return await promotionRepository.GetPromotionsByCategory(category);
        }

        public List<PromotionWithLocation> FilterPromotionWithLocationList(List<PromotionWithLocation> promoWithLocationList, Categories? category, string query)
        {
            query = query.ToLower();
            if(category == null)
            {
                return promoWithLocationList.Where(promo => (promo.Title.ToLower().Contains(query) || promo.Description.ToLower().Contains(query) || promo.CompanyName.ToLower().Contains(query))).ToList();
            }
            return promoWithLocationList.Where(promo => (promo.Title.ToLower().Contains(query) || promo.Description.ToLower().Contains(query) || promo.CompanyName.ToLower().Contains(query)) && promo.Category == category).ToList();           
        }

        public async Task<bool> StorePromotion(Promotion promotion)
        {
            return await promotionRepository.StorePromotion(promotion);
        }

        public async Task<List<Promotion>> SearchActivePromotions(string keyword)
        {
            return await promotionRepository.GetPromotions(true, keyword);
        }

        public async Task<List<PromotionWithLocation>> JoinPromotionWithLocation(List<Promotion> promos)
        {
            List<Vendor> allVendors = await vendorRepository.GetVendors();

            //Get unique vendors
            var uniqueVendors = allVendors.GroupBy(x => new { x.Location.Lat, x.Location.Lng }).Select(g => g.First()).ToList();


            var promotionsWithLocations = uniqueVendors.Join(promos, e => e.VendorId, b => b.VendorId,
                (e, b) => new PromotionWithLocation
                {
                    VendorId = b.VendorId,
                    Title = b.Title,
                    Location = e.Location,
                    Description = b.Description,
                    CompanyName = e.CompanyName,
                    Duration = 9999,
                    ImageURL = b.PromotionImageURL,
                    PromotionId = b.PromotionId,
                    PromotionStartDate = b.PromotionStartDate,
                    PromotionEndDate = b.PromotionEndDate,
                    Category = b.Category
                });

            return promotionsWithLocations.ToList();
        }

        public async Task<List<PromotionWithLocation>> GetActivePromotions()
        {
            List<Promotion> activePromotions = await promotionRepository.GetPromotions(true);
            List<Vendor> allVendors = await vendorRepository.GetVendors();

            //Get unique vendors
            var uniqueVendors = allVendors.GroupBy(x => new { x.Location.Lat, x.Location.Lng }).Select(g => g.First()).ToList();          
            

            var mapPromotions = uniqueVendors.Join(activePromotions, e => e.VendorId, b => b.VendorId,
                (e, b) => new PromotionWithLocation
                {
                    VendorId = b.VendorId,
                    Title = b.Title,
                    Location = e.Location,
                    Description = b.Description,
                    CompanyName = e.CompanyName,
                    Duration = 9999,
                    ImageURL = b.PromotionImageURL,
                    PromotionId = b.PromotionId,
                    PromotionStartDate = b.PromotionStartDate,
                    PromotionEndDate = b.PromotionEndDate,
                    Category = b.Category
                });

            //Select all promotions excluding those with empty locations
            var validatedMapPromotions = mapPromotions.Where(e => e.Location.Lat != 0 || e.Location.Lng != 0);

            return validatedMapPromotions.ToList();
        }

        public async Task<List<PromotionWithLocation>> PopulatePromotionWithLocationBlobs(List<PromotionWithLocation> promotionsWithLocation)
        {
            foreach(PromotionWithLocation promo in promotionsWithLocation)
            {
                if(promo.Image == null)
                {
                    promo.Image = await BlobClient.BlobClient.GetBlob(promo.ImageURL);
                }
            }

            return promotionsWithLocation;
        }

        public async Task DeletePromotion(Promotion promotion)
        {
            await promotionRepository.DeletePromotion(promotion);
        }

    }
}