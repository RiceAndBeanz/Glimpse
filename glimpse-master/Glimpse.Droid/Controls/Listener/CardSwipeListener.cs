using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Gemslibe.Xamarin.Droid.UI.SwipeCards;
using Glimpse.Droid.Controls;
using System.IO;
using Glimpse.Core.Repositories;
using Glimpse.Core.Contracts.Repository;
using Glimpse.Core.Model;
using Glimpse.Core.ViewModel;

namespace Glimpse.Droid.Controls.Listener
{
    

    public class CardSwipeListener : CardStack.ICardEventListener
    {
        private readonly int _discardDistancePx;
        private readonly CardStack _cardStack;
        private bool _swipeDiscard;
        private CustomViewPager _viewPager;
        private CardViewModel _cardViewModel;
        public event Action<string> OnCardSwipeActionEvent;
        private LocalPromotionRepository _localPromotionRepository;

        public CardSwipeListener(int discardDistancePx, CardStack cardStack, CustomViewPager viewPager, LocalPromotionRepository localPromotionRepository,  CardViewModel cardViewModel)
        {
            _discardDistancePx = discardDistancePx;
            _cardStack = cardStack;
            _viewPager = viewPager;
            _swipeDiscard = false;   
            _localPromotionRepository = localPromotionRepository;
            _cardViewModel = cardViewModel;
    }

        public bool SwipeEnd(int section, float x1, float y1, float x2, float y2)
        {
            //Discard card only if user moves card to Right or Left
            _swipeDiscard = Math.Abs(x2 - x1) > _discardDistancePx;
            var cardView = _cardStack.TopView as CustomCardView;
            if (_swipeDiscard)
            {
                var action = (x2 < x1) ? "Dislike" : "Like";
                OnCardSwipeActionEvent?.Invoke(action);
            }
            _viewPager.SetSwipeable(true);
            return _swipeDiscard;
        }

        public bool SwipeStart(int section, float x1, float y1, float x2, float y2)
        {
            _viewPager.SetSwipeable(false);
            return false;
        }

        public bool SwipeContinue(int section, float x1, float y1, float x2, float y2)
        {
            return false;
        }

        public void TopCardTapped()
        {
            PromotionWithLocation promotionWithLocation = (PromotionWithLocation)_cardStack.Adapter.GetItem(_cardStack.CurrIndex);
            _cardViewModel.ShowDetailPage(promotionWithLocation);
        }

       //for some reason discarding the card by swiping returns index+1.therefore this is my current solution for thios issue.
       //use do something to send promo to like list
        public void Discarded(int index, int direction)
        {
            if (_swipeDiscard)
            {
                saveLikedPromo(index - 1, direction);
                _swipeDiscard = false;
            }
            else
                saveLikedPromo(index, direction);
        }

        private async void saveLikedPromo(int index, int direction)
        {
            PromotionWithLocation promotionWithLocation = (PromotionWithLocation)_cardStack.Adapter.GetItem(index);  // to get discarded promotion 
            
            
            if (direction == 1 || direction == 3)
            {
                await _localPromotionRepository.Insert(promotionWithLocation);
            }
        }

    }
}