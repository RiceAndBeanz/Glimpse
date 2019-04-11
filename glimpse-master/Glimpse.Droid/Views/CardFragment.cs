using Android.OS;
using Android.Runtime;
using Android.Views;
using MvvmCross.Binding.Droid.BindingContext;
using MvvmCross.Droid.Shared.Attributes;
using MvvmCross.Droid.Support.V4;
using Glimpse.Core.ViewModel;
using Glimpse.Droid.Activities;
using Android.Widget;
using Glimpse.Core.Model;
using System.Collections.Generic;
using Glimpse.Droid.Adapter;
using Gemslibe.Xamarin.Droid.UI.SwipeCards;
using Glimpse.Droid.Controls;
using Android.Util;
using Android.Content;
using System.Threading.Tasks;
using Glimpse.Droid.Controls.Listener;
using Glimpse.Core.Contracts.Repository;
using Glimpse.Core.Repositories;
using System.IO;
using SQLite.Net.Platform.XamarinAndroid;
using MvvmCross.Binding.BindingContext;
using Glimpse.Droid.Helpers;
using MvvmCross.Core.ViewModels;
using MvvmCross.Platform.Droid.WeakSubscription;
using MvvmCross.Platform.WeakSubscription;

namespace Glimpse.Droid.Views
{
    [MvxFragment(typeof(MainViewModel), Resource.Id.viewPager, true)]
    [Register("glimpse.droid.views.CardFragment")]
    public class CardFragment : MvxFragment<CardViewModel>, RadioGroup.IOnCheckedChangeListener
    {
        private RadioGroup _radioGroup;
        private CardStack _cardStack;
        private CardAdapter _cardAdapter;
        private CustomViewPager _viewPager;
        private BindableProgress _bindableProgress;
        private LocalPromotionRepository _localPromotionRepository;
        private SearchView _searchView;
        private ImageButton _likeButton;
        private ImageButton _dislikeButton;
        private CardSwipeListener _cardSwipeListener;

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            base.OnCreateView(inflater, container, savedInstanceState);
            //obtaining view pager from parent fragment
            _viewPager = (CustomViewPager)container;
            return this.BindingInflate(Resource.Layout.CardSwipeView, null);
        }

        public override async void OnViewCreated(View view, Bundle savedInstanceState)
        {
            base.OnViewCreated(view, savedInstanceState);
            // (this.Activity as MainActivity).SetCustomTitle("Tiles");
            _radioGroup = (RadioGroup)view.FindViewById(Resource.Id.filter_radiogroup);
            _radioGroup.SetOnCheckedChangeListener(this);

            _cardStack = (this.Activity as MainActivity).FindViewById<CardStack>(Resource.Id.card_stack);
            _cardStack.ContentResource = Resource.Layout.Card_Layout;
            _cardAdapter = new CardAdapter(this.Context, Resource.Layout.Card_Layout, this.Activity);

            //create binding for progress
            _bindableProgress = new BindableProgress(this.Context);
            _bindableProgress.Title = ViewModel.TextSource.GetText("Progress");
            var set = this.CreateBindingSet<CardFragment, CardViewModel>();
            set.Bind(_bindableProgress).For(p => p.Visible).To(vm => vm.IsBusy);
            set.Apply();

            //initializing the repo to store liked promotions locally to pass to the card swipe listener
            _localPromotionRepository = new LocalPromotionRepository();
            var path = GetDbPath();
            await _localPromotionRepository.InitializeAsync(path, new SQLitePlatformAndroid());

            //Initializing the discard distance in pixels from the origin of the card stack.
            _cardSwipeListener = new CardSwipeListener(DpToPx(this.Context, 100), _cardStack, _viewPager, _localPromotionRepository, (CardViewModel)ViewModel);
            _cardStack.CardEventListener = _cardSwipeListener;

            //initializing the promotion list
            if (ViewModel.PromotionList == null)
            {
                List<PromotionWithLocation> alreadyLikedPromotions = await _localPromotionRepository.GetActivePromotions();
                await ViewModel.InitializeLocationAndPromotionList(alreadyLikedPromotions);
            }
                InitializeImages();


            IMvxNotifyPropertyChanged viewModel = ViewModel as IMvxNotifyPropertyChanged;
            viewModel.WeakSubscribe(PropertyChanged);
            _searchView = (SearchView)view.FindViewById(Resource.Id.card_searchview);


            //Subscribing to events
            _likeButton = view.FindViewById<ImageButton>(Resource.Id.btnLike);
            _dislikeButton = view.FindViewById<ImageButton>(Resource.Id.btnDislike);
            _likeButton.Click += LikeButton_Click;      
            _dislikeButton.Click += DislikeButton_Click;
            _cardSwipeListener.OnCardSwipeActionEvent += _cardSwipeListener_OnCardSwipeActionEvent;
            _searchView.SearchClick += delegate
            {
                _radioGroup.Visibility = ViewStates.Visible;
            };

            //done this weird way because of issue clearing the focus of the search view
            var listener = new MySearchViewOnCloseListener();
            listener.view = _radioGroup;
            _searchView.SetOnCloseListener(listener);

            _cardStack.Adapter = _cardAdapter;
        }

        private void PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "Query" || e.PropertyName == "SelectedItem")
            {
                InitializeImages();
            }
        }

        private void InitializeImages()
        {
            _cardAdapter.Clear();
            foreach (PromotionWithLocation promo in ViewModel.PromotionList)
            {
                _cardAdapter.Add(promo);
            }
        }


        private void _cardSwipeListener_OnCardSwipeActionEvent(string action)
        {
            PromotionWithLocation promotionWithLocation = (PromotionWithLocation)_cardStack.Adapter.GetItem(_cardStack.CurrIndex);
            ViewModel.PromotionList.Remove(promotionWithLocation);
            Toast.MakeText(this.Context, action , ToastLength.Short).Show();
        }

        private void LikeButton_Click(object sender, System.EventArgs e)
        {
            if (_cardStack.TopView != null)
            {
                PromotionWithLocation promotionWithLocation = (PromotionWithLocation)_cardStack.Adapter.GetItem(_cardStack.CurrIndex);
                ViewModel.PromotionList.Remove(promotionWithLocation);
                Toast.MakeText(this.Context, "Like", ToastLength.Short).Show();
                var direction = 3;
                Task.Delay(250).ContinueWith(o => { (this.Activity as MainActivity).RunOnUiThread(() => _cardStack.DiscardTop(direction, 700)); });
            }
            else
                Toast.MakeText(this.Context, "No cards available", ToastLength.Short).Show();
        }

        private void DislikeButton_Click(object sender, System.EventArgs e)
        {
            if (_cardStack.TopView != null)
            {
                PromotionWithLocation promotionWithLocation = (PromotionWithLocation)_cardStack.Adapter.GetItem(_cardStack.CurrIndex);
                ViewModel.PromotionList.Remove(promotionWithLocation);
                _cardAdapter.Remove(_cardStack.CurrIndex);
                Toast.MakeText(this.Context, "Dislike", ToastLength.Short).Show();
                var direction = 2;
                Task.Delay(250).ContinueWith(o => { (this.Activity as MainActivity).RunOnUiThread(() => _cardStack.DiscardTop(direction, 700)); });
            }
            else
                Toast.MakeText(this.Context, "No cards available", ToastLength.Short).Show();
        }

        public void OnCheckedChanged(RadioGroup group, int checkedId)
        {
            //radio group index is based on 1, making base 0
            checkedId = checkedId - 1;
            //the filter on previous page made this checkedID increment by 7...
            checkedId = checkedId % 7;
            if (checkedId == 0)
            {
                ViewModel.SelectedItem = null;
            }
            else
            {
                int checkedId0BasedIndex = checkedId - 1;
                Categories category = (Categories)checkedId0BasedIndex;
                ViewModel.SelectedItem = category;
            }
            InitializeImages();
        }


        public int DpToPx(Context context, int dip)
        {
            DisplayMetrics displayMetrics = context.Resources.DisplayMetrics;
            return (int)TypedValue.ApplyDimension(ComplexUnitType.Dip, dip, displayMetrics);
        }

        private string GetDbPath()
        {
            string documentsPath = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal);
            return Path.Combine(documentsPath, "glimpse.db3");
        }

    }
}