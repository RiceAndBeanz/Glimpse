using Glimpse.Core.Services.General;
using MvvmCross.Core.ViewModels;
using MvvmCross.Platform;
using MvvmCross.Plugins.Messenger;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace Glimpse.Core.ViewModel
{
    public class ViewPagerViewModel : BaseViewModel
    {
        private CardViewModel _cardViewModel;
        private MapViewModel _mapViewModel;
        private LikedPromotionsViewModel _likedPromotionsViewModel;

        public ViewPagerViewModel(IMvxMessenger messenger) : base(messenger)
        {
            _mapViewModel = (MapViewModel)Mvx.Resolve<IMvxViewModelLoader>().LoadViewModel(MvxViewModelRequest<MapViewModel>.GetDefaultRequest(), null);
            _cardViewModel = (CardViewModel)Mvx.Resolve<IMvxViewModelLoader>().LoadViewModel(MvxViewModelRequest<CardViewModel>.GetDefaultRequest(), null);
        }

        public MapViewModel MapViewModel
        {
            get
            { 
                if(_mapViewModel ==  null)
                    _mapViewModel = (MapViewModel)Mvx.Resolve<IMvxViewModelLoader>().LoadViewModel(MvxViewModelRequest<MapViewModel>.GetDefaultRequest(), null);

                return _mapViewModel;
            }
        }

        public CardViewModel CardViewModel
        {
            get
            {
                if(_cardViewModel == null)
                    _cardViewModel = _cardViewModel = (CardViewModel)Mvx.Resolve<IMvxViewModelLoader>().LoadViewModel(MvxViewModelRequest<CardViewModel>.GetDefaultRequest(), null);

                return _cardViewModel;

            }
        }

        public LikedPromotionsViewModel LikedPromotionsViewModel
        {
            get
            {
                if (_likedPromotionsViewModel == null)
                    _likedPromotionsViewModel = (LikedPromotionsViewModel)Mvx.Resolve<IMvxViewModelLoader>().LoadViewModel(MvxViewModelRequest<LikedPromotionsViewModel>.GetDefaultRequest(), null);

                return _likedPromotionsViewModel;
            }
        }


    }
}