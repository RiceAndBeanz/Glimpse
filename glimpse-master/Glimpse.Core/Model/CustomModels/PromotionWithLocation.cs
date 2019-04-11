using SQLite.Net.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Glimpse.Core.Model
{
    public class PromotionWithLocation
    {
        [PrimaryKey, AutoIncrement]
        public int PromotionWithLocationId { get; set; }
                
        public int VendorId { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }      

        public Categories Category { get; set; }

        public string CompanyName { get; set; }

        public int Duration { get; set; }

        public byte[] Image { get; set; }

        public string ImageURL { get; set; }

        public DateTime PromotionStartDate { get; set; }

        public DateTime PromotionEndDate { get; set; }

        public Location Location { get; set; }

        public int PromotionId { get; set; }
     //   public byte[] PromotionImage { get; set; }

    }
}
