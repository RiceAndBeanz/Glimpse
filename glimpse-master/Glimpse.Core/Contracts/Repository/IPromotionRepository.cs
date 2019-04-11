using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Glimpse.Core.Model;
using System.Collections.Generic;


namespace Glimpse.Core.Contracts.Repository
{
    public interface IPromotionRepository
    {
        Task<List<Promotion>> GetPromotion(int id);

        Task<List<Promotion>> GetPromotions(bool active = false, string keyword = "");

        Task<List<Promotion>> GetPromotionsByCategory(Categories category);

        Task<bool> StorePromotion(Promotion promotion);

        Task DeletePromotion(Promotion promotion);
    }
}