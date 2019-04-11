using Glimpse.Core.Contracts.Repository;
using Glimpse.Core.Model;
using Glimpse.Core.Repositories;
using Glimpse.Core.UnitTests.Mocks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Glimpse.Core.UnitTests.Tests.Repository
{
    [TestClass]
    public class LocalPromotionRepositoryTests
    {
        private LocalPromotionRepository _localPromotionRepository;

        [TestInitialize]
        public async void Initialize()
        {
            _localPromotionRepository = new LocalPromotionRepository();
            await _localPromotionRepository.InitializeAsync("unittest", new SQLite.Net.Platform.Generic.SQLitePlatformGeneric());
        }
        //SQLite.Net.Platform.Generic.SQLitePlatformGeneric()
        //ignore this because having trouble with the platfrom to mock.
        [Ignore]
        [TestMethod]
        public async Task TestGetActivePromotions()
        {
            //arrange
            Mock<ILocalPromotionRepository> mock = RepositoryMocks.GetMockLocalPromotionRepository(50);

            //act
            List<PromotionWithLocation> listOfPromo = await mock.Object.GetActivePromotions();

            //assert
            foreach (PromotionWithLocation promo in listOfPromo)
            {
                Assert.IsTrue(promo.PromotionStartDate < DateTime.Now && promo.PromotionEndDate > DateTime.Now);
            }

        }

    }
}
