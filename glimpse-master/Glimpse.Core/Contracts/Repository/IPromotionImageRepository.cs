using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Glimpse.Core.Model;
using System.Threading.Tasks;

namespace Glimpse.Core.Contracts.Repository
{
    public interface IPromotionImageRepository
    {
        Task<List<PromotionImage>> GetPromotionImage(int id);

        Task<List<PromotionImage>> GetPromotionImages();

        Task StorePromotionImage(PromotionImage promotionImage);
    }
}
