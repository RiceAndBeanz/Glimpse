using System;
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
using Glimpse.Core.Model;
using System.IO;
using Glimpse.Core.Contracts.Repository;
using Glimpse.Core.Repositories;
using Glimpse.Droid.Controls.Listener;
using SQLite.Net.Platform.XamarinAndroid;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace Glimpse.Droid.Views
{
    [MvxFragment(typeof(MainViewModel), Resource.Id.viewPager, true)]
    [Register("glimpse.droid.views.LikedPromotionsFragment")]
    public class LikedPromotionsFragment : MvxFragment<LikedPromotionsViewModel>, RadioGroup.IOnCheckedChangeListener, ListView.IOnScrollListener
    {
        private LocalPromotionRepository _localPromotionRepository;
        private RadioGroup _radioGroup;
        private SearchView _searchView;
        private ListView _listView;
        private bool _scrollEnabled;

        private MvxSwipeRefreshLayout _swipeRefreshLayout;
        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            base.OnCreateView(inflater, container, savedInstanceState);
            return this.BindingInflate(Resource.Layout.LikedPromotionsView, null);
        }

        public override void OnViewCreated(View view, Bundle savedInstanceState)
        {
            base.OnViewCreated(view, savedInstanceState);
            _swipeRefreshLayout = (MvxSwipeRefreshLayout)view.FindViewById(Resource.Id.refresher);
            _listView = (ListView)view.FindViewById(Resource.Id.listView_LikedItems);
            _listView.SetOnScrollListener(this);

            _searchView = (SearchView)View.FindViewById(Resource.Id.liked_searchview);
            _radioGroup = (RadioGroup)View.FindViewById(Resource.Id.filter_radiogroup);
           
            _searchView.SearchClick += delegate
            {
                _radioGroup.Visibility = ViewStates.Visible;
            };

            //done this weird way because of issue clearing the focus of the search view
            var listener = new MySearchViewOnCloseListener();
            listener.view = _radioGroup;           
            _searchView.SetOnCloseListener(listener); 
           
        }
      

    public override async void OnResume()
        {
            base.OnResume();

             _radioGroup = (RadioGroup)View.FindViewById(Resource.Id.filter_radiogroup);
             _radioGroup.SetOnCheckedChangeListener(this);
            
            _searchView.SetIconifiedByDefault(true);           

        }



        public async void ReloadPromotions()
        {           
            _localPromotionRepository = new LocalPromotionRepository();
            var path = GetDbPath();
            await _localPromotionRepository.InitializeAsync(path, new SQLitePlatformAndroid());
            List<PromotionWithLocation> activePromos = await _localPromotionRepository.GetActivePromotions();
            ViewModel.PromotionList = ViewModel.PromotionWithLocationToLikedItemWrap(activePromos);
            ViewModel.PromotionsStored = activePromos;
           
        }


        public void OnCheckedChanged(RadioGroup group, int checkedId)
        {
            //radio group index is based on 1, making base 0
            checkedId = checkedId - 1;
            //the filter on previous page made this checkedID increment by 7...
            checkedId = checkedId % 7;
            if (checkedId < 0)
                checkedId = checkedId + 7;

            if (checkedId == 0)
            {
                ViewModel.SelectedItem = null;
                _searchView.Visibility = ViewStates.Visible;
            }
            else
            {
                int checkedId0BasedIndex = checkedId - 1;
                Categories category = (Categories)checkedId0BasedIndex;
                ViewModel.SelectedItem = category;
            }
        }

        private string GetDbPath()
        {
            string documentsPath = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal);
            return Path.Combine(documentsPath, "glimpse.db3");
        }

        public void OnScroll(AbsListView view, int firstVisibleItem, int visibleItemCount, int totalItemCount)
        {
            int topRow= (_listView == null || _listView.ChildCount == 0) ? 0 : _listView.GetChildAt(0).Top;
            bool newScrollEnabled = (firstVisibleItem == 0 && topRow >= 0) ? true:false;

            if (null != _swipeRefreshLayout && _scrollEnabled != newScrollEnabled)
            {
                _swipeRefreshLayout.SetEnabled(newScrollEnabled);
                _scrollEnabled = newScrollEnabled;        
            }
        }

        public void OnScrollStateChanged(AbsListView view, [GeneratedEnum] ScrollState scrollState)
        {
  
        }
    }
}


