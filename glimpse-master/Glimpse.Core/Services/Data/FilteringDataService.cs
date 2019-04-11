using System.Collections.Generic;
using Glimpse.Core.Contracts.Repository;
using Glimpse.Core.Contracts.Services;
using Glimpse.Core.Model;

namespace Glimpse.Core.Services.Data
{
    public class FilteringDataService
    {
        private readonly IPromotionDataService promotionDataService;
        public FilteringDataService(IPromotionDataService promotionDataService)
        {
            this.promotionDataService = promotionDataService;
           
        }

/*
        public async List<Promotion> getFilteredPromotions()
        {
            List<Promotion> promotionsList = await promotionDataService.GetActivePromotions();
            var distinctItems = items.GroupBy(x => x.Id).Select(y => y.First());
        }
*/
    }
}