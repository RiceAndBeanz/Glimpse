using Android.OS;
using Android.Runtime;
using Android.Views;
using MvvmCross.Binding.Droid.BindingContext;
using MvvmCross.Droid.Shared.Attributes;
using MvvmCross.Droid.Support.V4;
using Glimpse.Core.ViewModel;
using Glimpse.Droid.Activities;
using Glimpse.Droid.Extensions;
using Glimpse.Droid.Adapter;
using System.Collections.Generic;
using Android.Support.V4.View;
using static Android.Support.V4.View.ViewPager;
using System;
using Glimpse.Droid.Controls;
using Android.Support.Design.Widget;
using Android.Graphics.Drawables;

namespace Glimpse.Droid.Views
{
    [MvxFragment(typeof(MainViewModel), Resource.Id.content_frame, true)]
    [Register("glimpse.droid.views.ViewPagerFragment")]
    public class ViewPagerFragment : MvxFragment<ViewPagerViewModel>, IOnPageChangeListener, MainActivity.OnBackPressedListener
    {
        private CustomViewPager _viewPager;
        private TabLayout _tabLayout;
        private MvxViewPagerFragmentAdapter _adapter;
        private int[] _tabIconsGrey = { Resource.Drawable.ic_thumbs_up_down_dark_grey, Resource.Drawable.ic_playlist_add_check_grey_24px, Resource.Drawable.ic_location_dark_grey };
        private int[] _tabIconsGreen = { Resource.Drawable.ic_thumbs_up_down_green, Resource.Drawable.ic_playlist_add_check_green_24px, Resource.Drawable.ic_location_green };


        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            base.OnCreateView(inflater, container, savedInstanceState);
            return this.BindingInflate(Resource.Layout.ViewPagerView, null);
        }

        public override void OnViewCreated(View view, Bundle savedInstanceState)
        {
            base.OnViewCreated(view, savedInstanceState);
            (this.Activity as MainActivity).setOnBackPressedListener(this);
            (this.Activity as MainActivity).SetCustomTitle("Map");

            var fragments = new List<MvxViewPagerFragmentAdapter.FragmentInfo>
                  {                   
                    new MvxViewPagerFragmentAdapter.FragmentInfo
                    {
                      FragmentType = typeof(Views.CardFragment),
                      ViewModel = ViewModel.CardViewModel
                    },
                    new MvxViewPagerFragmentAdapter.FragmentInfo
                    {
                      FragmentType = typeof(Views.LikedPromotionsFragment),
                      ViewModel = ViewModel.LikedPromotionsViewModel
                    },
                     new MvxViewPagerFragmentAdapter.FragmentInfo
                    {
                      FragmentType = typeof(Views.MapFragment),
                      ViewModel = ViewModel.MapViewModel
                     }
                  };

            _viewPager = View.FindViewById<CustomViewPager>(Resource.Id.viewPager);
            _adapter = new MvxViewPagerFragmentAdapter(this.Context, ChildFragmentManager, fragments);
            _viewPager.Adapter = _adapter;
            _viewPager.AddOnPageChangeListener(this);
            _tabLayout = View.FindViewById<TabLayout>(Resource.Id.tabs);
            _tabLayout.SetupWithViewPager(_viewPager);

            _tabLayout.GetTabAt(0).SetIcon(_tabIconsGreen[0]);
            _tabLayout.GetTabAt(1).SetIcon(_tabIconsGrey[1]);
            _tabLayout.GetTabAt(2).SetIcon(_tabIconsGrey[2]);

            ViewModel.LikedPromotionsViewModel.LocationUpdate += LikedPromotionsViewModel_LocationUpdate;
        }

        private void LikedPromotionsViewModel_LocationUpdate(object sender, Core.Helpers.LocationChangedHandlerArgs e)
        {
            _viewPager.SetCurrentItem(2, true);
            ViewModel.MapViewModel.Location = e.Location;
        }

        public void OnPageScrollStateChanged(int state)
        {
           // throw new NotImplementedException();
        }

        public void OnPageScrolled(int position, float positionOffset, int positionOffsetPixels)
        {

        }

        public void OnPageSelected(int position)
        {
            for (int i = 0; i < _tabIconsGrey.Length; i++)
            {
                _tabLayout.GetTabAt(i).SetIcon(_tabIconsGrey[i]);
            }
            _tabLayout.GetTabAt(position).SetIcon(_tabIconsGreen[position]); 

            if(position == 1)
            {
                var pagerliked = (LikedPromotionsFragment)_adapter.GetItem(1);
                pagerliked.ReloadPromotions();
            }            
        }

        public void doBack()
        {

            if (_viewPager.CurrentItem == 1 && _viewPager.IsShown)
            {
                _viewPager.SetCurrentItem(0, true);
            }
           else if (_viewPager.CurrentItem == 2 && _viewPager.IsShown)
            {
                _viewPager.SetCurrentItem(1, true);
            }
            else
            {
                (this.Activity as MainActivity).setOnBackPressedListener(null);
                (this.Activity as MainActivity).OnBackPressed();
            }
        }

    }
}