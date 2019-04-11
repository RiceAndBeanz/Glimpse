using Glimpse.Core.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Glimpse.Core.Contracts.Services
{
    public interface IPromotionClickDataService
    {
        Task StorePromotionClick(PromotionClick promotionClick);

        Task<List<PromotionClick>> GetPromotionClick(int id);

        Task<List<PromotionClick>> GetPromotionClicks();

        Task DeletePromotionClick(PromotionClick promotionClick);
    }
}
