using Glimpse.Core.Contracts.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Glimpse.Core.Model;
using Plugin.RestClient;

namespace Glimpse.Core.Repositories
{
    public class PromotionClickRepository : IPromotionClickRepository
    {
        public async Task<List<PromotionClick>> GetPromotionClick(int id)
        {
            RestClient<PromotionClick> restClient = new RestClient<PromotionClick>();

            var promotionClick = await restClient.GetByIdAsync(id);

            return promotionClick;
        }

        public async Task<List<PromotionClick>> GetPromotionClicks()
        {
            RestClient<PromotionClick> restClient = new RestClient<PromotionClick>();

            var promotion = await restClient.GetAsync();

            return promotion;
        }

        public async Task StorePromotionClick(PromotionClick promotionClick)
        {
            RestClient<PromotionClick> restClient = new RestClient<PromotionClick>();

            await restClient.PostAsync(promotionClick);
        }

        public async Task DeletePromotionClick(PromotionClick promotionClick)
        {
            RestClient<PromotionClick> restClient = new RestClient<PromotionClick>();

            await restClient.DeleteAsync(promotionClick.PromotionClickId, promotionClick);
        }
    }
}
