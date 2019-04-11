using Glimpse.Core.Contracts.Repository;
using Glimpse.Core.Model;
using Plugin.RestClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Glimpse.Core.Repositories
{
    public class PromotionImageRepository : IPromotionImageRepository
    {
        public async Task StorePromotionImage(PromotionImage promotionImage)
        {
            RestClient<PromotionImage> restClient = new RestClient<PromotionImage>();

            await restClient.PostAsync(promotionImage);
        }

        public async Task<List<PromotionImage>> GetPromotionImage(int id)
        {
            RestClient<PromotionImage> restClient = new RestClient<PromotionImage>();

            var promotion = await restClient.GetByIdAsync(id);

            return promotion;
        }

        public async Task<List<PromotionImage>> GetPromotionImages()
        {
            RestClient<PromotionImage> restClient = new RestClient<PromotionImage>();

            var promotion = await restClient.GetAsync();

            return promotion;
        }
    }
}
