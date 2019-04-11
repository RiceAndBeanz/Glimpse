using Glimpse.Core.Contracts.Repository;
using Glimpse.Core.Contracts.Services;
using Glimpse.Core.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Glimpse.Core.Services.Data
{
    public class PromotionClickDataService : IPromotionClickDataService
    {

        private readonly IPromotionClickRepository promotionClickRepository;

        public PromotionClickDataService(IPromotionClickRepository promotionClickRepository)
        {
            this.promotionClickRepository = promotionClickRepository;

        }

        public async Task<List<PromotionClick>> GetPromotionClick(int id)
        {
            return await promotionClickRepository.GetPromotionClick(id);
        }

        public async Task<List<PromotionClick>> GetPromotionClicks()
        {
            return await promotionClickRepository.GetPromotionClicks();
        }

        public async Task StorePromotionClick(PromotionClick promotionClick)
        {
            await promotionClickRepository.StorePromotionClick(promotionClick);
        }

        public async Task DeletePromotionClick(PromotionClick promotionClick)
        {
            await promotionClickRepository.DeletePromotionClick(promotionClick);
        }
    }
}
