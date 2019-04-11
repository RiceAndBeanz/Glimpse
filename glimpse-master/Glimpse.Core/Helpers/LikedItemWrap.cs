using Glimpse.Core.Model;
using Glimpse.Core.ViewModel;
using MvvmCross.Core.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Glimpse.Core.Helpers
{
    public class LikedItemWrap
    {
        PromotionWithLocation _mnuItem;
        LikedPromotionsViewModel _parent;

        public LikedItemWrap(PromotionWithLocation item, LikedPromotionsViewModel parent)
        {
            _mnuItem = item;
            _parent = parent;
        }


        public IMvxCommand OrderClick
        {
            get
            {
                return new MvxCommand(() => _parent.btnClick(_mnuItem));
            }
        }

        public PromotionWithLocation Item { get { return _mnuItem; } }
    }
}
