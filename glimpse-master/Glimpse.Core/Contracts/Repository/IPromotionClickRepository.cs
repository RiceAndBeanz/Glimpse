using Glimpse.Core.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Glimpse.Core.Contracts.Repository
{
    public interface IPromotionClickRepository
    {
        Task<List<PromotionClick>> GetPromotionClick(int id);

        Task<List<PromotionClick>> GetPromotionClicks();

        Task StorePromotionClick(PromotionClick promotionClick);

        Task DeletePromotionClick(PromotionClick promotionClick);
    }
}
