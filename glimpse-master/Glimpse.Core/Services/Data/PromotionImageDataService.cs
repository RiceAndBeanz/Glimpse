using Glimpse.Core.Contracts.Repository;
using Glimpse.Core.Contracts.Services;
using Glimpse.Core.Helpers;
using Glimpse.Core.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Glimpse.Core.Services.Data
{
    public class PromotionImageDataService : IPromotionImageDataService
    {

        private readonly IPromotionImageRepository promotionImageRepository;       

        public PromotionImageDataService(IPromotionImageRepository promotionImageRepository)
        {
            this.promotionImageRepository = promotionImageRepository;
          
        }

        public async Task<List<PromotionImage>> GetPromotionImage(int id)
        {
            return await promotionImageRepository.GetPromotionImage(id);
        }

        public async Task<List<byte[]>> GetImageListFromPromotionWithLocationId(int id)
        {
            List<PromotionImage> allPromotionImages = (await GetPromotionImages()).Where(x => (x.PromotionId == id)).ToList();
            List<byte[]> listofImages = new List<byte[]>();
            foreach (PromotionImage pi in allPromotionImages)
            {
                listofImages.Add(await BlobClient.BlobClient.GetBlob(pi.ImageURL));
            }

            return listofImages;
        }

        public async Task<List<PromotionImage>> GetPromotionImages()
        {
            return await promotionImageRepository.GetPromotionImages();
        }

        public async Task StorePromotion(PromotionImage promotionImage)
        {
            await promotionImageRepository.StorePromotionImage(promotionImage);
        }


    }
}
