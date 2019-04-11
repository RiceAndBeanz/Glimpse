using System;
using MvvmCross.Core.ViewModels;
using MvvmCross.Platform;
using Glimpse.Core.Contracts.ViewModel;
using Glimpse.Core.ViewModel;
using Glimpse.Core.Services;
using Glimpse.Core.Services.Data;
using Glimpse.Core.Repositories;

namespace Glimpse.Core.ViewModel
{
    public class MainViewModel : MvxViewModel, IMainViewModel
    {
        private Lazy<ViewPagerViewModel> viewPager;
        private bool glimpseMode = false;

        public MainViewModel()
        {
            viewPager = new Lazy<ViewPagerViewModel>(Mvx.IocConstruct<ViewPagerViewModel>);
        }

       public void Init(bool glimpseMode)
        {
            this.glimpseMode = glimpseMode;
        }

        public void ShowMenu()
        {
            ShowViewModel<MenuViewModel>();
        }

        public void ShowViewPager()

        {
            ShowViewModel<ViewPagerViewModel>();
        }

        public void ShowImageSlider()

        {
            ShowViewModel<PromoDetailsViewModel>();
        }

        public bool GlimpseMode
        {
            get { return glimpseMode; }
            set { glimpseMode = value; }
        }
    }
}