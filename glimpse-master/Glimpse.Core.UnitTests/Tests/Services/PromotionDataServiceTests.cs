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
    public class PromotionDataServiceTests
    {
        private PromotionDataService _pds;

        [TestInitialize]
        public void Initialize()
        {
            PromotionRepository promotionRepo = new PromotionRepository();
            VendorRepository vendorRepo = new VendorRepository();
            _pds = new PromotionDataService(promotionRepo, vendorRepo);
        }

        [TestMethod]
        public async Task GetActivePromotions_Returns_Valid()
        {
            //arrange

            //act
            List<PromotionWithLocation> activePromos = await _pds.GetActivePromotions();
            //assert

            foreach(PromotionWithLocation promo in activePromos)
            {
                Assert.IsTrue(promo.PromotionStartDate < DateTime.Now && promo.PromotionEndDate > DateTime.Now);                   
            }

        }

        [TestMethod]
        public async Task SearchActivePromotions_Returns_Valid()
        {
            //arrange
            string keyword = "p";

            //act
            List<Promotion> activePromos = await _pds.SearchActivePromotions(keyword);
            //assert

            foreach (Promotion promo in activePromos)
            {
                Assert.IsTrue(promo.PromotionStartDate < DateTime.Now && promo.PromotionEndDate > DateTime.Now);
                Assert.IsTrue(promo.Title.ToLower().Contains(keyword) || promo.Description.ToLower().Contains(keyword));
            }

        }

        [TestMethod]
        public async Task GetPromotionsByCategory_Returns_GoodCategory()
        {
            //arrange
            Categories category = Categories.Footwear;

            //act
            List<Promotion> categoryPromos = await _pds.GetPromotionsByCategory(category);
            //assert

            foreach (Promotion promo in categoryPromos)
            {
                Assert.IsTrue(promo.Category == category);
            }        
        }

        //This method does not require access to the database
        [TestMethod]
        public void FilterPromotionWithLocationList_Returns_GoodCategory()
        {
            //arrange
            Categories category = Categories.Footwear;
            List<PromotionWithLocation> promosWithLocation = new List<PromotionWithLocation>();

            for(int i = 0; i <= 10; i++)
            {
                promosWithLocation.Add(new PromotionWithLocation
                {
                    VendorId = 1,
                    Title = "Unit Test Title",
                    Description = "Unit Test Description",
                    Category = category,
                    CompanyName = "Unit Test Company",
                    Duration = 2
                });
            }

            for (int i = 0; i <= 10; i++)
            {
                promosWithLocation.Add(new PromotionWithLocation
                {
                    VendorId = 1,
                    Title = "Unit Test Title",
                    Description = "Unit Test Description",
                    Category = Categories.Apparel,
                    CompanyName = "Unit Test Company",
                    Duration = 2
                });
            }

            for (int i = 0; i <= 10; i++)
            {
                promosWithLocation.Add(new PromotionWithLocation
                {
                    VendorId = 1,
                    Title = "Unit Test Title",
                    Description = "Unit Test Description",
                    Category = Categories.Jewellery,
                    CompanyName = "Unit Test Company",
                    Duration = 2
                });
            }

            //act
            List<PromotionWithLocation> categoryPromos =  _pds.FilterPromotionWithLocationList(promosWithLocation, category, "");
            //assert

            foreach (PromotionWithLocation promo in categoryPromos)
            {
                Assert.IsTrue(promo.Category == category);
            }
        }


        [TestMethod]
        public void FilterPromotionWithLocationList_All_Returns_AllCategories()
        {
            //arrange
            Categories category1 = Categories.Footwear;
            Categories category2 = Categories.Restaurants;
            Categories category3 = Categories.Jewellery;
            List<PromotionWithLocation> promosWithLocation = new List<PromotionWithLocation>();

            for (int i = 0; i < 10; i++)
            {
                promosWithLocation.Add(new PromotionWithLocation
                {
                    VendorId = 1,
                    Title = "Unit Test Title",
                    Description = "Unit Test Description",
                    Category = category1,
                    CompanyName = "Unit Test Company",
                    Duration = 2
                });
            }

            for (int i = 0; i < 10; i++)
            {
                promosWithLocation.Add(new PromotionWithLocation
                {
                    VendorId = 1,
                    Title = "Unit Test Title",
                    Description = "Unit Test Description",
                    Category = category2,
                    CompanyName = "Unit Test Company",
                    Duration = 2
                });
            }

            for (int i = 0; i < 10; i++)
            {
                promosWithLocation.Add(new PromotionWithLocation
                {
                    VendorId = 1,
                    Title = "Unit Test Title",
                    Description = "Unit Test Description",
                    Category = category3,
                    CompanyName = "Unit Test Company",
                    Duration = 2
                });
            }

            //act
            List<PromotionWithLocation> categoryPromos = _pds.FilterPromotionWithLocationList(promosWithLocation, null, "");
            //assert

            foreach (PromotionWithLocation promo in categoryPromos)
            {
                Assert.IsTrue(promo.Category == category1 || promo.Category == category2 || promo.Category == category3);
            }

            Assert.AreEqual(10, categoryPromos.Count(promo => promo.Category == category1));
            Assert.AreEqual(10, categoryPromos.Count(promo => promo.Category == category2));
            Assert.AreEqual(10, categoryPromos.Count(promo => promo.Category == category3));
        }

    }
}
