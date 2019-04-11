using System.Collections.Generic;
using Moq;
using Glimpse.Core.Contracts.Repository;
using Glimpse.Core.Model;
using System;

namespace Glimpse.Core.UnitTests.Mocks
{
    public class RepositoryMocks
    {
        private static Random random = new Random();

        public static Mock<ILocalPromotionRepository> GetMockLocalPromotionRepository(int count)
        {
           
            var list = new List<PromotionWithLocation>();
            var mockLocalPromotionRepository = new Mock<ILocalPromotionRepository>();

            for (int i = 0; i < count; i++)
            {
                list.Add(
                    new PromotionWithLocation
                    {
                        PromotionStartDate = DateTime.Now.AddDays(random.Next(30) - 15),
                        PromotionEndDate = DateTime.Now.AddDays(random.Next(30))
                    });
            }

            mockLocalPromotionRepository.Setup(m => m.GetPromotions()).ReturnsAsync(list);
         
            return mockLocalPromotionRepository;
        }
      
    }
}