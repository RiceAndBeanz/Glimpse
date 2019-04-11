using Glimpse.Core.Contracts.Services;
using SQLite.Net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Glimpse.Core.Model;
using MvvmCross.Platform;
using SQLite.Net.Async;
using Glimpse.Core.Contracts.Repository;

namespace Glimpse.Core.Services.Data.Local
{
    public class LocalPromotionDataService : ILocalPromotionDataService
    {
        private readonly ILocalPromotionRepository _localPromotionRepository;

        public LocalPromotionDataService(ILocalPromotionRepository localPromotionRepository)
        {
            _localPromotionRepository = localPromotionRepository;
        }

        public async Task Delete(PromotionWithLocation promotionWithLocation)
        {
            await _localPromotionRepository.Delete(promotionWithLocation);
        }

        public async Task Insert(PromotionWithLocation promotionWithLocation)
        {
            await _localPromotionRepository.Insert(promotionWithLocation);
        }

        public async Task<List<PromotionWithLocation>> GetPromotions()
        {
            return await _localPromotionRepository.GetPromotions();
        }




    }
}
