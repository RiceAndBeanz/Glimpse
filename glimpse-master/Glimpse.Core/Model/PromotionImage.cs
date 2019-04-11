using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Glimpse.Core.Model
{
    public class PromotionImage
    {              
        public int PromotionImageId { get; set; }

        public byte[] Image { get; set; }

        public int PromotionId { get; set; }

        public string ImageURL { get; set; }
    }
}
