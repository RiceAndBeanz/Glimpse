using Glimpse.Core.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Glimpse.Core.Contracts.Repository
{
    public interface ILocalPromotionRepository
    {
        Task<PromotionWithLocation> Insert(PromotionWithLocation promotionWithLocation);

        Task<PromotionWithLocation> Delete(PromotionWithLocation promotionWithLocation);

        Task<List<PromotionWithLocation>> GetPromotions();

        Task<List<PromotionWithLocation>> GetActivePromotions();

    }
}
