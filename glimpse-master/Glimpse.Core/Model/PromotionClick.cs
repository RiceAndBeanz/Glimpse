﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Glimpse.Core.Model
{
    public class PromotionClick
    {       
        public int PromotionClickId { get; set; }

        public DateTime Time { get; set; }

        public int PromotionId { get; set; }
    }
}
