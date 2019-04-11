using System;
using System.Threading.Tasks;
using Glimpse.Core.Contracts.Repository;
using Glimpse.Core.Model;
using Glimpse.Core.Repositories;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;

namespace Glimpse.Core.UnitTests.Tests.Repository
{
    [TestClass]
    public class PromotionRepositoryTests
    {
        
        IPromotionRepository repository;
        IVendorRepository vendorRepository;

        [TestInitialize]
        public void Initialize()
        {
            repository = new PromotionRepository();
            vendorRepository = new VendorRepository();
        }

        [TestMethod]
        public async Task Test_GetPromotions_Return_All_Promotions()
        {
            //Act
            var promotions = await repository.GetPromotions();

            //Assert
            Assert.AreNotEqual(0, promotions.Count);
        }


        [TestMethod]
        public async Task Test_StorePromotion_Creates_Promotion()
        {
            //arrange
            var promotionsBefore = await repository.GetPromotions();
            var promotionsCountBefore = promotionsBefore.Count;
            var unitTestByteArray = new byte[3000];
            for (int i = 0; i < unitTestByteArray.Length; i++)
            {
                unitTestByteArray[i] = 0x20;
            }

            //getting a vendor to attach promotion too, promo will be deleted after
            List<Vendor> vendors = await vendorRepository.GetVendors();
            int vendorId = vendors[0].VendorId;

            Promotion promotion = new Promotion()
            {
                Title = "TestPromotion",
                Description = "PromotionDescription",
                Category = Categories.Apparel,
                PromotionStartDate = DateTime.Now,
                PromotionEndDate = DateTime.Now.AddDays(4),
                VendorId = vendorId,
                PromotionImage = unitTestByteArray,
                PromotionImageURL = "unitTest",
            };

            //act 
            bool promotionCreated = await repository.StorePromotion(promotion);

            //Assert
            var promotionsAfter = await repository.GetPromotions();
            var promotionsCountAfter = promotionsAfter.Count;

            var difference = promotionsCountAfter - promotionsCountBefore;
            Assert.IsTrue(difference == 1);

            //cleanup
            await repository.DeletePromotion(promotionsAfter[promotionsAfter.Count - 1]);

        }       
        
    }
}
