using Glimpse.Core.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Glimpse.Core.Contracts.Services
{
    public interface ILocalPromotionDataService
    {
        Task Insert(PromotionWithLocation promotionWithLocation);

        Task Delete(PromotionWithLocation promotionWithLocation);

        Task<List<PromotionWithLocation>> GetPromotions();
    }
}
