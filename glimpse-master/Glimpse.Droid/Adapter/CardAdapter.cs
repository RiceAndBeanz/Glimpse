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
using Glimpse.Core.Model;
using Glimpse.Droid.Controls;
using Android.Graphics;
using static Android.Graphics.Bitmap;


namespace Glimpse.Droid.Adapter
{
    public class CardAdapter : CardStackAdapter
    {
        Activity _activity; 

        public CardAdapter(Context context, int resource, Activity activity) :base(context,resource)
        {
            _activity = activity;
        }


        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            return convertView;
        }

        public override async void BindView(int position, View convertView, ViewGroup parent)
        {
            var pwl = (PromotionWithLocation)GetItem(position);
            var cardTitle = convertView.FindViewById<TextView>(Resource.Id.cardTitle);
            Typeface tf0 = Typeface.CreateFromAsset(_activity.Assets, "Avenir-Light.otf");
            cardTitle.SetTypeface(tf0, TypefaceStyle.Normal);

            var cardImage = convertView.FindViewById<ImageView>(Resource.Id.cardImage);

            if (pwl.Title != null)
               cardTitle.Text = pwl.Title;

            if (pwl.Image != null)
                cardImage.SetImageBitmap(RoundCornerImage(BitmapFactory.DecodeByteArray(pwl.Image, 0, pwl.Image.Length), 20));

        }
        public Bitmap RoundCornerImage(Bitmap raw, float round)
        {
            int width = raw.Width;
            int height = raw.Height;
            Bitmap result = Bitmap.CreateBitmap(width, height, Config.Argb8888);
            Canvas canvas = new Canvas(result);
            canvas.DrawARGB(0, 0, 0, 0);

            Paint paint = new Paint();
            paint.AntiAlias = true; 
            paint.Color=Color.ParseColor("#000000");

            Rect rect = new Rect(0, 0, width, height);
            RectF rectF = new RectF(rect);

            canvas.DrawRoundRect(rectF, round, round, paint);

            paint.SetXfermode(new PorterDuffXfermode(PorterDuff.Mode.SrcIn));
            canvas.DrawBitmap(raw, rect, rect, paint);

            return result;
        }
    }
}