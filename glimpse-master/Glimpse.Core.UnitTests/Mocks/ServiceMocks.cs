using System.Collections.Generic;
using Moq;
using Glimpse.Core.Contracts.Repository;
using Glimpse.Core.Model;
using Glimpse.Core.Repositories;
using Glimpse.Core.Services.Data;

namespace Glimpse.Core.UnitTests.Mocks
{
    public class ServiceMocks
    {   
        /*
        public static StoreDataService GetMockStoreDataService(int count)
        {
            var list = new List<Store>();

            var mockStoreRepository = new Mock<IStoreRepository>();
            for (int i = 0; i < count; i++)
            {
                list.Add(new Store { });
            }
            mockStoreRepository.Setup(m => m.GetAllStores()).ReturnsAsync(list);

            var storeDataService = new StoreDataService(mockStoreRepository.Object);
            return storeDataService;
        }

        public static LoginDataService GetMockLoginDataService()
        {
            var mockUserRepository = new Mock<UserRepository>();
            var mockVendorRepository = new Mock<VendorRepository>();
            var loginDataService = new LoginDataService(mockUserRepository.Object,mockVendorRepository.Object);
            return loginDataService;
        }
        */
    }
}