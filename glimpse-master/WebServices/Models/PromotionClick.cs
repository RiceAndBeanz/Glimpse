using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebServices.Models
{
    public class PromotionClick
    {
        [Key]
        public int PromotionClickId { get; set; }

        public DateTime Time { get; set; }

        public int PromotionId { get; set; }

        public virtual Promotion Promotion { get; set; }
    }
}