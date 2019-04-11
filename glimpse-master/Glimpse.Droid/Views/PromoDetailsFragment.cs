using Android.OS;
using Android.Runtime;
using Android.Views;
using MvvmCross.Binding.Droid.BindingContext;
using MvvmCross.Droid.Shared.Attributes;
using MvvmCross.Droid.Support.V4;
using Glimpse.Core.ViewModel;
using Glimpse.Droid.Activities;
using Glimpse.Droid.Extensions;
using Android.Widget;
using Glimpse.Droid.Adapter;
using Android.Support.V4.View;
using System;
using System.IO;
using System.Collections.Generic;
using Android.Graphics;
using Android.App;
using SupportToolbar = Android.Support.V7.Widget.Toolbar;
using Android.Support.V7.App;
using Android.Support.Design.Widget;

namespace Glimpse.Droid.Views
{
    [MvxFragment(typeof(MainViewModel), Resource.Id.content_frame, true)]
    [Register("glimpse.droid.views.PromoDetailsFragment")]
    public class PromoDetailsFragment : MvxFragment<PromoDetailsViewModel>, ViewPager.IOnPageChangeListener
    { 
        protected View _view;
        private int _dotsCount;
        private ImageView[] _dots;
        private LinearLayout _dotsLinearLayout;
        private ViewPager _viewPager;
        private SlidingImageAdapter _adapter;
        private List<Bitmap> _ImageResources;
        private List<byte[]> _byteImages;
        private TextView _description;
        private TextView _duration;
        private TextView _title;

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            base.OnCreateView(inflater, container, savedInstanceState);
            return this.BindingInflate(Resource.Layout.PromoDetailsView, null);
        }

        public override async void OnViewCreated(View view, Bundle savedInstanceState)
        {
            base.OnViewCreated(view, savedInstanceState);
            (this.Activity as MainActivity).SetCustomTitle("Details");

            /*            SupportToolbar toolbar = View.FindViewById<SupportToolbar>(Resource.Id.toolbar);
                        (this.Activity as AppCompatActivity).SetSupportActionBar(toolbar);
                        (this.Activity as AppCompatActivity).SupportActionBar.SetDisplayHomeAsUpEnabled(true); 

                        string promotionName = ViewModel.PromotionTitle;

                        CollapsingToolbarLayout collapsingToolbar = View.FindViewById<CollapsingToolbarLayout>(Resource.Id.collapsing_toolbar);
                        collapsingToolbar.Title = promotionName; 
                        */
            AppBarLayout appbarlayout = View.FindViewById<AppBarLayout>(Resource.Id.app_bar_layout);
            appbarlayout.LayoutParameters.Height = Resources.DisplayMetrics.WidthPixels;
            RelativeLayout relativelayout = View.FindViewById<RelativeLayout>(Resource.Id.dotsRelativeLayout);
            ((CollapsingToolbarLayout.LayoutParams)relativelayout.LayoutParameters).SetMargins(0, Resources.DisplayMetrics.WidthPixels-50, 0, 0);
            await LoadImageList();
            SetupViewPagerAndAdapter();
            SetupDotsControl();


           


        }

        public override async void OnStart()
        {
            base.OnStart();
      
        }

        public async override void OnResume()
        {
            base.OnResume();
            await LoadImageList();
        }

        private async System.Threading.Tasks.Task LoadImageList()
        {
            //Create a progress dialog for loading
            ProgressDialog pr = new ProgressDialog(this.Context);
            pr.SetMessage(ViewModel.TextSource.GetText("Progress"));
            pr.SetCancelable(false);

            var viewModel = (PromoDetailsViewModel)ViewModel;
            pr.Show();
            //Get the images
            _byteImages = await viewModel.GetImageList();
            pr.Hide();
        }

       public void SetupViewPagerAndAdapter()
        {
            _ImageResources = new List<Bitmap> ();
            foreach (byte[] image in _byteImages)
            {
                if(image != null)
                    _ImageResources.Add(BitmapFactory.DecodeByteArray(image, 0, image.Length));
            }

            _adapter = new SlidingImageAdapter(this.Context, _ImageResources);
            _viewPager = (ViewPager)View.FindViewById(Resource.Id.imagesViewPager);
            _viewPager.Adapter = _adapter;
            _viewPager.SetOnPageChangeListener(this);
        }

        private void SetupDotsControl()
        {
            _dotsLinearLayout = (LinearLayout)View.FindViewById(Resource.Id.viewPagerCountDots);
            _dotsCount = _adapter.Count;
            _dots = new ImageView[_dotsCount];

            if (_dotsCount != 0)
            {
                for (int i = 0; i < _dotsCount; i++)
                {
                    _dots[i] = new ImageView(this.Context);
                    _dots[i].SetImageDrawable(Resources.GetDrawable(Resource.Drawable.nonselecteditem_dot));
                    LinearLayout.LayoutParams p = new LinearLayout.LayoutParams(LinearLayout.LayoutParams.WrapContent, LinearLayout.LayoutParams.WrapContent);
                    p.SetMargins(4, 0, 4, 0);
                    _dotsLinearLayout.AddView(_dots[i], p);
                }
                _dots[0].SetImageDrawable(Resources.GetDrawable(Resource.Drawable.selecteditem_dot));
            }
        } 
        private async void  LoadImagesIfNeeded()
        {
            //Create a progress dialog for loading
            ProgressDialog pr = new ProgressDialog(this.Context);
            pr.SetMessage(ViewModel.TextSource.GetText("Progress"));
            pr.SetCancelable(false);

            var viewModel = (PromoDetailsViewModel)ViewModel;
            pr.Show();
            //Get the images
            _byteImages = await viewModel.GetImageList();
            pr.Hide();
        }

        public void OnPageScrollStateChanged(int state)
        {
            //throw new NotImplementedException();
        }

        public void OnPageScrolled(int position, float positionOffset, int positionOffsetPixels)
        {
           // throw new NotImplementedException();
        }

        public void OnPageSelected(int position)
        {
            if (_dotsCount != 0)
            {
                for (int i = 0; i < _dotsCount; i++)
                {
                    _dots[i].SetImageDrawable(Resources.GetDrawable(Resource.Drawable.nonselecteditem_dot));
                }
                _dots[position].SetImageDrawable(Resources.GetDrawable(Resource.Drawable.selecteditem_dot));
            }
        }



    }
}