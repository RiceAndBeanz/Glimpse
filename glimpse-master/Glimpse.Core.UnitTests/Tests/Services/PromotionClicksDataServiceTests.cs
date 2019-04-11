using Glimpse.Core.Model;
using Glimpse.Core.Repositories;
using Glimpse.Core.Services.Data;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Glimpse.Core.UnitTests.Tests.Services
{
    [TestClass]
    public class PromotionClicksDataServiceTests
    {
        private PromotionClickDataService _pcds;
        private PromotionDataService _pds;

        [TestInitialize]
        public void Initialize()
        {
            PromotionClickRepository promoClickRepo = new PromotionClickRepository();
            _pcds = new PromotionClickDataService(promoClickRepo);

            PromotionRepository promotionRepo = new PromotionRepository();
            VendorRepository vendorRepo = new VendorRepository();
            _pds = new PromotionDataService(promotionRepo, vendorRepo);
        }

        [TestMethod]
        public async Task CreatePromotionClick_Successful()
        {
            //arrange

            //getting a promotion to attach too
            List<Promotion> promos = await _pds.GetPromotions();
            int promoId = promos[0].PromotionId;

            PromotionClick promoClick = new PromotionClick
            {
                PromotionId = promoId,
                Time = DateTime.Now
            };

            var allPromoClicksBefore = await _pcds.GetPromotionClicks();
            int countBefore = allPromoClicksBefore.Count;

            //act
            await _pcds.StorePromotionClick(promoClick);

            //assert

            var allPromoClicksAfter = await _pcds.GetPromotionClicks();
            int countAfter = allPromoClicksAfter.Count;

            Assert.AreEqual(1, countAfter - countBefore);

            //cleanup

            await _pcds.DeletePromotionClick(allPromoClicksAfter[countAfter-1]);
        }
    }
}
