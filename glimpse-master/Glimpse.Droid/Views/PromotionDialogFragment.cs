using System.Collections.Generic;
using Android.App;
using Android.Graphics;
using Android.OS;
using Android.Provider;
using Android.Support.V7.Widget;
using Android.Views;
using Android.Widget;
using Glimpse.Core.Helpers;
using Glimpse.Core.Model;
using Glimpse.Droid.Adapter;
using Glimpse.Droid.Helpers;

namespace Glimpse.Droid.Views
{
    public class PromotionDialogFragment : DialogFragment
    {
        private readonly string title;
        private readonly string description;
        private readonly int expirationDate;
        private readonly string companyName;
        private Bitmap image;

        private RecyclerView mRecyclerView;
        private RecyclerView.LayoutManager mLayoutManager;
        private RecyclerView.Adapter mAdapter;
        private readonly List<PromotionWithLocation> currentPromotion;

        public PromotionDialogFragment(PromotionItem item)
        {
            currentPromotion = new List<PromotionWithLocation> { item.CurrentPromotion };
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            base.OnCreateView(inflater, container, savedInstanceState);
            
            var view = inflater.Inflate(Resource.Layout.PromotionDialogView, container, false);

            mRecyclerView = view.FindViewById<RecyclerView>(Resource.Id.recyclerView);

                //Create our layout manager
                mLayoutManager = new LinearLayoutManager(Application.Context);
                mRecyclerView.SetLayoutManager(mLayoutManager);
                mAdapter = new PromotionDialogRecyclerAdapter(currentPromotion, mRecyclerView, Application.Context);
                mRecyclerView.SetAdapter(mAdapter);

            return view;
        }

      
    }
}