using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Android.Animation;
using Android.App;
using Android.Content;
using Android.Content.Res;
using Android.Graphics;
using Android.OS;
using Android.Provider;
using Android.Runtime;
using Android.Support.V7.Widget;
using Android.Views;
using Android.Widget;
using Glimpse.Core.BlobClient;
using Glimpse.Core.Model;


namespace Glimpse.Droid.Adapter
{
    public class PromotionDialogRecyclerAdapter : RecyclerView.Adapter
    {
        private List<PromotionWithLocation> currentPromotion;
        private RecyclerView recyclerView;
        private Context context;
        private int mCurrentPosition = -1;
        
        BlobClient _client = new BlobClient();

        
        public PromotionDialogRecyclerAdapter(List<PromotionWithLocation> promotion, RecyclerView recyclerView, Context context)
        {
            this.currentPromotion = new List<PromotionWithLocation>();
            currentPromotion.AddRange(promotion);
            this.recyclerView = recyclerView;
            this.context = context;
        }

        public class MyView : RecyclerView.ViewHolder
        {
            public MyView(View view) : base(view)
            {
                mMainView = view;
            }

            public View mMainView { get; set; }
            public TextView CompanyNameTextView { get; set; }
            public TextView TitleTextView { get; set; }
            public TextView DescriptionTextView { get; set; }
            public ImageView PromotionImageView { get; set; }
            public TextView ExpirationDateTextView { get; set; }
        }


        public override int GetItemViewType(int position)
        {
                return Resource.Layout.PromotionDialogRecyclerViewRow;     
        }

        public override RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType)
        {
                //First card view
                View row = LayoutInflater.From(parent.Context).Inflate(Resource.Layout.PromotionDialogRecyclerViewRow, parent, false);

                TextView txtCompanyName = row.FindViewById<TextView>(Resource.Id.txtCompanyName);
                TextView txtTitle = row.FindViewById<TextView>(Resource.Id.txtTitle);
                TextView txtDescription = row.FindViewById<TextView>(Resource.Id.txtDescription);
                TextView txtExpiration = row.FindViewById <TextView>(Resource.Id.txtExpiration);
                ImageView imageViewPromotion = row.FindViewById<ImageView>(Resource.Id.promotionImage);


                MyView view = new MyView(row)
                {
                    CompanyNameTextView = txtCompanyName,
                    TitleTextView = txtTitle,
                    DescriptionTextView = txtDescription,
                    ExpirationDateTextView = txtExpiration,
                    PromotionImageView = imageViewPromotion
                };
                return view;            
        }

        public override async void OnBindViewHolder(RecyclerView.ViewHolder holder, int position)
        {
                MyView myHolder = holder as MyView;
                myHolder.CompanyNameTextView.Text = currentPromotion[position].CompanyName;
                myHolder.TitleTextView.Text = currentPromotion[position].Title;
                myHolder.DescriptionTextView.Text = currentPromotion[position].Description;

                byte[] promotionImage = await BlobClient.GetBlob(currentPromotion[position].ImageURL);

                myHolder.PromotionImageView.SetImageBitmap(BitmapFactory.DecodeByteArray(promotionImage, 0, promotionImage.Length));
            myHolder.ExpirationDateTextView.Text = "Expiring " + currentPromotion[position].PromotionEndDate.ToString("MMMM dd, yyyy");



            if (position > mCurrentPosition)
                {
                    int currentAnim = Resource.Animation.slide_left_to_right;
                    SetAnimation(myHolder.mMainView, currentAnim);
                    mCurrentPosition = position;
                }
            

        }

        private void SetAnimation(View view, int currentAnim)
        {
            Animator animator = AnimatorInflater.LoadAnimator(context, Resource.Animation.flip);
            animator.SetTarget(view);
            animator.Start();
            //Animation anim = AnimationUtils.LoadAnimation(context, currentAnim);
            //view.StartAnimation(anim);
        }

        public override int ItemCount
        {
            get { return this.currentPromotion.Count; }
        }
    }
}