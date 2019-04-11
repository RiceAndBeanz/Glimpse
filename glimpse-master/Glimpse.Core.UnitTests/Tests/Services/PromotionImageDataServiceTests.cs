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
    public class PromotionImageDataServiceTests
    {
        private PromotionImageDataService _pids;
        private PromotionDataService _pds;

        [TestInitialize]
        public void Initialize()
        {
            PromotionRepository promotionRepo = new PromotionRepository();
            VendorRepository vendorRepo = new VendorRepository();
            _pds = new PromotionDataService(promotionRepo, vendorRepo);
            PromotionImageRepository promotionImageRepo = new PromotionImageRepository();  
            _pids = new PromotionImageDataService(promotionImageRepo);
          
        }

        [TestMethod]
        public async Task GetImageListFromPromotionWithLocationId_Returns_Valid()
        {
            //arrange
            List<PromotionWithLocation> activePromos = await _pds.GetActivePromotions();
            int activePromoID = activePromos[0].PromotionId;
           
            //act
            List<byte[]> imageListFromActivePromoId = await _pids.GetImageListFromPromotionWithLocationId(activePromoID);
            List<PromotionImage> filteredPromotionImagesByActivePromoId = (await _pids.GetPromotionImages()).Where(p => p.PromotionImageId == activePromoID ).ToList();
            
            //assert        
            foreach (PromotionImage promotionImage in filteredPromotionImagesByActivePromoId)
            {
                Assert.IsTrue(imageListFromActivePromoId.Contains(promotionImage.Image));
            }
        }
    }
}
